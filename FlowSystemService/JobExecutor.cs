using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Parsing;
using Easis.Wfs.FlowSystemService.PESAbstraction.LongRunning;
using Easis.Wfs.FlowSystemService.PESAbstraction.Storage;
using Easis.Wfs.FlowSystemService.Utils;
using Easis.Wfs.Interpreting;
using Easis.Wfs.Interpreting.Utils;
using Eventing;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using NLog;
using Easis.Common.DataContracts;

namespace Easis.Wfs.FlowSystemService
{

    /// <summary>
    /// Class for management of disconnected jobs
    /// provide interface for free threads to get ready jobs
    /// </summary>
    internal class DisconnectedJobCollection : IEventConsumer
    {
        private List<Pair<Job, bool>> JobsReadyness = new List<Pair<Job, bool>>();
        private object SyncObject = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Job if ready job exists and null overwise</returns>
        public Job TryTakeReadyJob()
        {
            lock (SyncObject)
            {
                var ret = JobsReadyness.FirstOrDefault(pair => pair.Second == true);
                if (ret != null)
                {
                    JobsReadyness.Remove(ret);
                    return ret.First;
                }
                else
                    return null;
            }
        }

        public void AddDisconnectedJob(Job job)
        {
            lock (SyncObject)
            {
                JobsReadyness.Add(new Pair<Job, bool>(job, false));
            }
        }

        public void PushEvent(FlowEvent flowEvent)
        {
            lock (SyncObject)
            {
                var readyJob = JobsReadyness.FirstOrDefault(pair => pair.First.WfId == flowEvent.WfId);
                if (readyJob != null)
                {
                    readyJob.Second = true;
                }
            }
        }
    }

    /// <summary>
    /// Основной класс сервиса. FlowSystemService является оберткой для данного класса. Содержит логику многопоточной обработки задач.
    /// Реализован по шаблону проектирования одиночка (singleton). Поддерживает многопоточный доступ.
    /// </summary>
    public class JobExecutor : IEventConsumer
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        #region Singleton
        private static volatile JobExecutor _instance;
        private static readonly object _syncRoot = new Object();

        private JobExecutor()
        {
            Initialize();
        }

        public static JobExecutor Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new JobExecutor();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Очередь поступающих задач.
        /// Поддерживает многопоточный доступ.
        /// </summary>
        private SyncronizedBlockingQueue<Job> _queue = new SyncronizedBlockingQueue<Job>();

        private DisconnectedJobCollection _disconnectedJobs = new DisconnectedJobCollection();

        // С помощью него осуществляется проброс внешних событий
        // TODO: заменить dict на безопасный dict
        private IDictionary<Guid, Job> _startedJobs = new Dictionary<Guid, Job>();

        // TODO: memory leaking
        private IList<Job> _finishedJobs = new List<Job>();

        #region ThreadPool
        //TODO: Threadpool of workers
        private Thread[] _workerThreads = null;
        private int _threadCount = 0;

        /// <summary>
        /// Функция потока обработки запросов
        /// </summary>
        private void WorkerFunc()
        {
            _log.Info("[thread: {0}] Worker thread started", Thread.CurrentThread.ManagedThreadId);
            while (true)
            {
//                _log.Debug("[thread: {0}] Waiting for job", Thread.CurrentThread.ManagedThreadId);

                bool isNewJob;
                Job job = null;

                //--------------------------------------
                // GETTING JOB FROM THE 2 SOURCES
                //--------------------------------------
                // try to take job from disconnected
                var readyJob = _disconnectedJobs.TryTakeReadyJob();

                if (readyJob != null)
                {
                    _log.Info("[thread: {0}] WF#{1} was taken from disconnected jobs", Thread.CurrentThread.ManagedThreadId, readyJob.WfId);
                    job = readyJob;
                    isNewJob = false;
                }
                // no ready jobs, go to new
                else
                {
                    // try to take new job
                    try
                    {
                        //TODO: move to settings
                        job = _queue.Dequeue(new TimeSpan(0, 0, 10));
                        isNewJob = true;
                    }
                    catch (TimeoutException)
                    {
                        // try to take from another source
                        continue;
                    }
                }
                // job is taken

                WfLog _wflog = new WfLog(_log, job.WfId);

                //--------------------------------------
                // PRE actions for new job
                //--------------------------------------
                if (isNewJob)
                {
                    // переносим в список исполняющихся заданий
                    // TODO: проверить дубликат
                    _startedJobs.Add(job.WfId, job);

                    _log.Info("[thread: {0}] new job WF#{1} was dequeued", Thread.CurrentThread.ManagedThreadId,
                              job.WfId);

                    EventingService.EventingBrokerServiceClient proxy = null;
                    WFStateUpdatedEvent ev = null;
                    EventReport er = null;

                    #region Eventing

                    try
                    {
                        // generating event
                        proxy = new EventingService.EventingBrokerServiceClient();

                        ev = new WFStateUpdatedEvent();
                        ev.WFRunCode = job.WfId.ToString();
                        ev.WFStepCode = "";
                        ev.WFStateUpdatedType = WFStateUpdatedTypeEnum.WFStarted;
                        ev.Comment = "Interpreter thread id: " + Thread.CurrentThread.ManagedThreadId;

                        er = new EventReport()
                                 {
                                     Source = "Interpreter",
                                     Body =
                                         EventReportSerializer.SerializeObject(ev, typeof(WFStateUpdatedEvent)),
                                     SchemeUri =
                                         "http://escience.ifmo.ru/easis/eventing/schemes/WFStateUpdatedEvent.xsd",
                                     Timestamp = DateTime.Now,
                                     Topic = "WFStateUpdatedEvent"
                                 };
                        try
                        {
                            proxy.FireEvent(er);
                            _wflog.Trace("[eventing] Generated {0}", er.ToString());
                        }
                        catch (Exception ex)
                        {
                            _log.ErrorException("Error while generating 'WF start' event. Ignoring.", ex);
                            _wflog.ErrorException("Error while generating 'WF start' event. Ignoring.", ex);
                            //TODO: continue execution?
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.ErrorException("Error while generating 'WF start' event. Ignoring.", ex);
                        _wflog.ErrorException("Error while generating 'WF start' event. Ignoring.", ex);
                    }

                    #endregion
                }

                //--------------------------------------
                // MAIN ACTIONS
                //--------------------------------------
                bool wasFinished = false;
                try
                {
                    if (isNewJob)
                        job.Execute(_flowSystemContext, _stepStarter, _storage, _longRunningController);
                    else
                        job.ThreadReConnect();

                    wasFinished = true;
                }
                catch (ThreadDisconnectedException)
                {
                    _log.Trace("The job has been added to disconnected list. Thread is free.");
                    _wflog.Trace("The job has been added to disconnected list. Thread is free.");
                    _disconnectedJobs.AddDisconnectedJob(job);
                }
                catch (Exception ex)
                {
                    _wflog.ErrorException("Error during job execution", ex);
                    _log.ErrorException("Error during job execution", ex);
                }

                //--------------------------------------
                // POST actions
                //--------------------------------------
                if (wasFinished)
                {
                    #region Resulting

                    try
                    {
                        MonitoringFacade.MonitoringServiceClient moncli =
                            new MonitoringFacade.MonitoringServiceClient();

                        try
                        {
                            var jd = job.GetDescription();
                            moncli.PutJobResult(jd);
                            _wflog.Trace("Results were commited to provenance");
                        }
                        catch (Exception ex)
                        {
                            // переносим в список завершенных заданий
                            _finishedJobs.Add(job);
                            _log.ErrorException("Error while constructing result. Ignoring. Alarm! Job has been transmited to _finishedJobs. It smels like memory leaking.", ex);
                            _wflog.ErrorException("Error while constructing result. Ignoring. Alarm! Job has been transmited to _finishedJobs. It smels like memory leaking.", ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.ErrorException("Error while constructing result. Ignoring.", ex);
                        _wflog.ErrorException("Error while constructing result. Ignoring.", ex);
                    }

                    #endregion

                    #region Eventing

                    try
                    {
                        EventingService.EventingBrokerServiceClient proxy = null;
                        WFStateUpdatedEvent ev = null;
                        EventReport er = null;

                        // generating event
                        proxy = new EventingService.EventingBrokerServiceClient();

                        ev = new WFStateUpdatedEvent();
                        ev.WFRunCode = job.WfId.ToString();
                        ev.WFStepCode = "";
                        ev.WFStateUpdatedType = WFStateUpdatedTypeEnum.WFFinished;
                        ev.Comment = "Interpreter thread id: " + Thread.CurrentThread.ManagedThreadId;

                        er = new EventReport()
                                 {
                                     Source = "Interpreter",
                                     Body = EventReportSerializer.SerializeObject(ev, typeof(WFStateUpdatedEvent)),
                                     SchemeUri =
                                         "http://escience.ifmo.ru/easis/eventing/schemes/WFStateUpdatedEvent.xsd",
                                     Timestamp = DateTime.Now,
                                     Topic = "WFStateUpdatedEvent"
                                 };
                        try
                        {
                            proxy.FireEvent(er);
                            _wflog.Trace("[eventing] Generated {0}", er.ToString());
                        }
                        catch (Exception ex)
                        {
                            _log.ErrorException("Error while generating 'WF stop' event. Ignoring.", ex);
                            _wflog.ErrorException("Error while generating 'WF stop' event. Ignoring.", ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.ErrorException("Error while generating 'WF stop' event. Ignoring.", ex);
                        _wflog.ErrorException("Error while generating 'WF stop' event. Ignoring.", ex);
                    }

                    #endregion

                    // TODO: проверить наличие
                    _startedJobs.Remove(job.WfId);

                    _log.Info("[thread: {0}] Finished execution of WF#{1}", Thread.CurrentThread.ManagedThreadId, job.WfId);
                    _wflog.Info("[thread: {0}] Finished execution of WF#{1}", Thread.CurrentThread.ManagedThreadId, job.WfId);
                }
            }
        }

        #endregion

        #region Resulting
        private BsonDocument HelperJsonToBson(string json)
        {
            JObject jo = JObject.Parse(json);

            MemoryStream ms = new MemoryStream();
            BsonWriter writer = new BsonWriter(ms);

            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, jo);

            byte[] bytes = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(bytes, 0, (int)ms.Length);

            BsonDocument bdquery = BsonSerializer.Deserialize<BsonDocument>(bytes);
            return bdquery;
        }

        #endregion

        #region external configured params
        /// <summary>
        /// Sequence id generator
        /// </summary>
        private IIdGenerator<Guid> _idGenerator = null;
        //private IIdGenerator _idGenerator = null;

        /// <summary>
        /// Step starter implementor
        /// </summary>
        private IStepStarter _stepStarter = null;
        private ILongRunningController _longRunningController = null;
        private IStorage _storage = null;

        private FlowSystemContext _flowSystemContext = null;
        #endregion


        #region configuration
        /// <summary>
        /// Конфигурация сервиса, создание потоков обработки, привязка механизмов работы с CLAVIRE.
        /// </summary>
        private void Initialize()
        {
            _log.Info("Initialization");

            //_idGenerator = new SimpleIdGenerator();
            _idGenerator = new GuidGenerator();

            //// PES
            ////_stepStarter = new PesStepStarter();

            // Execution
            _stepStarter = new DryExecutionStepStarter(null); // new ExecutionStepStarter();
            _stepStarter.IsDry = false;

            _storage = new FedorStorage((string)Properties.Settings.Default["StorageUri"]);

            _longRunningController = new ZmqLongRunningController();

            _flowSystemContext = new FlowSystemContext();

            //Threadpool initialization
            _threadCount = Properties.Settings.Default.ThreadCount;
            _workerThreads = new Thread[_threadCount];

            // ThreadPool start workers
            for (int i = 0; i < _threadCount; i++)
            {
                _workerThreads[i] = new Thread(new ThreadStart(WorkerFunc));
                _workerThreads[i].Start();
            }

            _log.Trace(_threadCount + " threads started");
        }
        #endregion

        #region public interface

        /// <summary>
        /// Постановка задачи в очередь на обработку. Проверка корректности jobRequest. Присвоение заданию идентификатора.
        /// </summary>
        /// <param name="jobRequest">Задание</param>
        /// <returns>Идентификатор задания</returns>
        public Guid PushJob(JobRequest jobRequest)
        {
            if (!CheckJobRequest(jobRequest))
                throw new ArgumentException("Job request is not correct. More info in debug mode logs.");

            _log.Trace("Pushing new job");
            // _log.Trace(jobRequest.Script);
            // _log.Trace(jobRequest.ScriptDataContext);

            Guid ret = _idGenerator.NextId();
            _queue.Enqueue(new Job(jobRequest, ret));

            _log.Info("New job pushed with id = {0}, Request.DataContext = {1}", ret, jobRequest.ScriptDataContext);

            return ret;
        }
        #endregion

        #region validation
        /// <summary>
        /// Базовая проверка корректности запроса на исполнение задания. 
        /// Производится перед присваиванием ID и формированием объекта Job.
        /// </summary>
        /// <param name="jobRequest"></param>
        /// <returns></returns>
        private bool CheckJobRequest(JobRequest jobRequest)
        {
            //TODO: realize check
            return true;
        }
        #endregion

        /// <summary>
        /// Регистрация внешнего события. Событие маршрутизируется к WF.
        /// </summary>
        /// <param name="flowEvent">Событие</param>
        public void PushEvent(FlowEvent flowEvent)
        {
            if (_startedJobs.ContainsKey(flowEvent.WfId))
            {
                _log.Debug("Got PushEvent command for WF#{0}. {1}", flowEvent.WfId, flowEvent.ToString());
                _startedJobs[flowEvent.WfId].GetEventConsumer().PushEvent(flowEvent);
            }
            else
            {
                _log.Error("Got PushEvent command for not active WF#{0}. Ignoring. {1}", flowEvent.WfId, flowEvent.ToString());
            }

            // mark readyness of disconnected job
            _disconnectedJobs.PushEvent(flowEvent);

        }
        /// <summary>
        /// Команда паузы. Маршрутизируется к WF или конкретному узлу.
        /// </summary>
        /// <param name="WfId">Идентификатор WF</param>
        /// <param name="blockId">Идентификатор узла</param>
        public void Pause(Guid WfId, long blockId = -1)
        {
            if (_startedJobs.ContainsKey(WfId))
            {
                _log.Debug("Got Pause command for WF#{0}. {1}", WfId, blockId);
                if (blockId == -1)
                    _startedJobs[WfId].Pause();
                else
                    _startedJobs[WfId].Pause(blockId);
            }
            else
            {
                _log.Error("Got Pause command for not active WF#{0} - Ignoring.", WfId);
            }
        }
        /// <summary>
        /// Команда возобновления работы. Маршрутизируется к WF или конкретному узлу.
        /// </summary>
        /// <param name="WfId">Идентификатор WF</param>
        /// <param name="blockId">Идентификатор узла</param>
        public void Resume(Guid WfId, long blockId = -1)
        {
            if (_startedJobs.ContainsKey(WfId))
            {
                _log.Debug("Got Resume command for WF#{0}. {1}", WfId, blockId);
                if (blockId == -1)
                    _startedJobs[WfId].Resume();
                else
                    _startedJobs[WfId].Resume(blockId);
            }
            else
            {
                _log.Error("Got Resume command for not active WF#{0} - Ignoring.", WfId);
            }
        }
        /// <summary>
        /// Команда остановки. Маршрутизируется к WF или конкретному узлу.
        /// </summary>
        /// <param name="WfId">Идентификатор WF</param>
        /// <param name="blockId">Идентификатор узла</param>
        public void Stop(Guid WfId, long blockId = -1)
        {
            if (_startedJobs.ContainsKey(WfId))
            {
                _log.Debug("Got Stop command for WF#{0}. {1}", WfId, blockId);
                if (blockId == -1)
                    _startedJobs[WfId].Stop();
                else
                    _startedJobs[WfId].Stop(blockId);
            }
            else
            {
                _log.Error("Got Stop command for not active WF#{0} - Ignoring.", WfId);
            }
        }
        /// <summary>
        /// Получение полной информации о состоянии конкретного WF.
        /// </summary>
        /// <param name="wfId">Идентификатор WF</param>
        /// <returns>Информация о WF</returns>
        public JobDescription GetJobDescription(Guid wfId)
        {
            lock (_syncRoot)
            {
                lock (_queue.SyncRoot)
                {
                    foreach (var p in _queue)
                    {
                        if (p.WfId == wfId)
                            return p.GetDescription();
                    }
                }
                if (_startedJobs.ContainsKey(wfId))
                    return _startedJobs[wfId].GetDescription();

                foreach (var p in _finishedJobs)
                {
                    if (p.WfId == wfId)
                        return p.GetDescription();
                }
            }

            throw new ObjectNotFoundException();
        }
        /// <summary>
        /// Получение полной информации о состоянии всех активных WF.
        /// </summary>
        /// <returns>Список контейнеров с данными о WF</returns>
        public JobDescription[] GetJobDescriptions()
        {
            IList<JobDescription> ret = new List<JobDescription>();
            lock (_syncRoot)
            {
                lock (_queue.SyncRoot)
                {
                    foreach (var p in _queue)
                    {
                        ret.Add(p.GetDescription());
                    }
                }

                foreach (var value in _startedJobs.Values)
                {
                    ret.Add(value.GetDescription());
                }

                foreach (var p in _finishedJobs)
                {
                    ret.Add(p.GetDescription());
                }

                return ret.ToArray();
            }
        }
        /// <summary>
        /// Получение информации о задачах длительного исполнения.
        /// </summary>
        /// <returns>Хэш</returns>
        public Dictionary<Guid, List<LongRunningStepRunInfo>> GetLongRunningRunInfos()
        {
            Dictionary<Guid, List<LongRunningStepRunInfo>> ret = new Dictionary<Guid, List<LongRunningStepRunInfo>>();
            lock (_syncRoot)
            {
                foreach (var value in _startedJobs.Values)
                {
                    List<LongRunningStepRunInfo> p = value.GetLongRunningRunInfos();
                    if (p.Count > 0)
                    {
                        ret.Add(value.WfId, p);
                    }
                }
            }
            return ret;
        }
    }
}