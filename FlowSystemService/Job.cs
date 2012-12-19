using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Easis.Common.DataContracts;
using Easis.Common.Exceptions;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Parsing;
using Easis.Wfs.FlowSystemService.ExecutionService;
using Easis.Wfs.FlowSystemService.InfrastructureLevel.LongRunning;
using Easis.Wfs.FlowSystemService.InfrastructureLevel.Storage;
using Easis.Wfs.Interpreting;
using Newtonsoft.Json.Linq;
using NLog;
using System.Text.RegularExpressions;
using Easis.Common;
using ExecutionContext = Easis.Common.DataContracts.ExecutionContext;

namespace Easis.Wfs.FlowSystemService
{
    /// <summary>
    /// Задание на исполнение WF. Содержит информацию о WF. Несет функциональность по подготовке к запуску, исполнению WF. Использует компонент разбора скрипта EasyFlow.
    /// Поддерживает многопоточный доступ.
    /// </summary>
    public sealed class Job : IControllable, IController, IThreadConnectable //IJsonMonitorable
    {
        WfLog _log = null;
        /// <summary>
        /// Описание ошибки для пользователя
        /// </summary>
        public string ErrorUserComment { get; private set; }
        /// <summary>
        /// Подробный комментарий к ошибке
        /// </summary>
        public string VerboseErrorComment { get; private set; }
        /// <summary>
        /// Исключение, вызвавшее ошибку
        /// </summary>
        public Exception ErrorException { get; private set; }

        /// <summary>
        /// Объект синхронизации для вызова методов из других потоков
        /// напр., проверка состояния задачи
        /// </summary>
        private object _syncExt = new object();

        /// <summary>
        /// Состояние шага
        /// </summary>
        private JobState _jobState = JobState.Created;


        #region Входные данные
        private JobRequest _jobRequest;
        private Guid _wfId;
        private FlowSystemContext _flowSystemContext = null;
        private IStepStarter _stepStarter = null;
        private ILongRunningController _longRunningController = null;
        private IStorage _storage = null;
        #endregion

        #region Промежуточные данные
        private ParseResult _parseResult = null;
        private FlowDataContext _flowDataContext = null;
        private ExecutionContext _flowExecutionProperties = null;
        private IEventConsumer _eventConsumer = null;

        // TODO: временно, на WF визуализатор
        private DeclarativeInterpreter _declarativeInterpreter = null;

        private DateTime _timestampPushed = DateTime.MinValue;
        private DateTime _timestampStarted = DateTime.MinValue;
        private DateTime _timestampFinished = DateTime.MinValue;

        #endregion


        /// <summary>
        /// Разбор скрипта
        /// Формирует: _parseResult
        /// </summary>
        protected void ParseScript()
        {
            string scriptText;
            scriptText = JobRequest.Script;

            State = JobState.Parsing;
            _log.Debug("[thread: {0}]  Script parsing for WF#{1} started", Thread.CurrentThread.ManagedThreadId, this.WfId);
            // Парсинг скрипта
            ParserSettings settings = new ParserSettings
            {
                CollectScopeInformation = true,
                CollectTextInformation = true,
                LogRuleTraces = true
            };
            ScriptParser parser = new ScriptParser(settings);
            ParseResult parseResult = parser.Parse(scriptText);

            if (parseResult.Succeeded)
            {
                _log.Debug("[thread: {0}]  Parsing succeeded", Thread.CurrentThread.ManagedThreadId);

                this._parseResult = parseResult;

                State = JobState.Parsed;
            }
            else
            {
                ErrorUserComment = "Parsing error";
                _log.Info("[thread: {0}] WF#{1} execution failed while parsing script", Thread.CurrentThread.ManagedThreadId, this.WfId);

                string parserMessages = String.Join("\n",
                            parseResult.ParserMessageCollection.Select(
                                message =>
                                string.Format("  {0} ({1}): {2}", message.MessageType,
                                              message.MessageObject.TextRange.Start, message.Message)));

                VerboseErrorComment = parserMessages;

                State = JobState.Error;

                _log.Debug("[thread: {0}] WF#{1} script parsing errors:\n {2}", Thread.CurrentThread.ManagedThreadId, this.WfId, parserMessages);

                //TODO: записать куда-то результат History?
                //TODO: form jobResult
            }
        }

        /// <summary>
        /// Разбор контекстов исполнения и данных. Проверка корректности контекстов.
        /// Формирует: _flowDataContext, _flowExecutionProperties
        /// </summary>
        protected void ValidateModel()
        {
            State = JobState.Validating;
            _log.Debug("[thread: {0}]  Script model validating", Thread.CurrentThread.ManagedThreadId);

            Flow flow = _parseResult.ScriptModel.MainFlow;

            // Формируем FlowDataContext

            // TODO: IoC
            _log.Trace("Parsing data context");
            IDataContextExtractor dataContextExtractor = new DataContextExtractor();
            try
            {
                _flowDataContext = dataContextExtractor.CreateDataContext(JobRequest.ScriptDataContext);
            }
            catch (InvalidFormatException e)
            {
                _log.Error("[thread: {0}]  Error while parsing FlowDataContext", Thread.CurrentThread.ManagedThreadId);

                VerboseErrorComment = String.Format("Invalid data context: '{0}'", JobRequest.ScriptDataContext);

                throw e;
            }

            try
            {
                // Формируем FlowExecutionProperties

                //_flowExecutionProperties = dataContextExtractor.CreateExecutionContext(JobRequest.FlowExecutionProperties);

                _flowExecutionProperties = dataContextExtractor.CreateExecutionContext(JobRequest.ExecutionProperties);
            }
            catch (Exception ex)
            {
                _log.Error("[thread: {0}]  Error while parsing FlowExecutionContext", Thread.CurrentThread.ManagedThreadId);
                VerboseErrorComment = String.Format("Invalid exec context: '{0}'", JobRequest.FlowExecutionProperties);
                //throw ex;
            }

            // check WF for correctness
            bool wfIsCorrect = true;

            // TODO: check requirements in JobRequest.ScriptDataContext

            // TODO: check ScriptDataContext

            if (wfIsCorrect)
            {
                _log.Debug("[thread: {0}]  Script model validated successfully", Thread.CurrentThread.ManagedThreadId);
                State = JobState.Validated;
            }
            else
            {
                ErrorUserComment = "Job validation failed";
                State = JobState.Error;
                _log.Info("[thread: {0}] WF#{1} execution failed while checking job for correctness", Thread.CurrentThread.ManagedThreadId, WfId);
                //TODO: записать куда-то результат History?
                //TODO: form job result
            }
        }

        // TODO: refactor. durty hack. Need normal outputs in Result
        private FileDescriptor[] _fileDescriptors = null;

        /// <summary>
        /// Первичное исполнение WF. Использует DeclarativeInterpreter с фиктивными механизмами IStepStarter, IStorage, ILongRunningController
        /// </summary>
        public void DryRun()
        {
            IStepStarter stepStarter = new DryExecutionStepStarter(null);

            // check timeout
            TimeSpan timeout = Properties.Settings.Default.ThreadWaitEventTimeout;

            // Создаем контекст
            IGlobalContext gc = new GlobalContext
            {
                FileRegistry = new DictBasedFileRegistry(),
                FlowSystemContext = _flowSystemContext,
                PackageRegistry = new ListBasedPackageRegistry(),
                StepStarter = stepStarter,
                LongRunningController = new DryRunLongRunningController(),
                Storage = new DruRunStorage(),
                ThreadWaitForEventTimeout = timeout
            };

            DeclarativeInterpreter declarativeInterpreter = new DeclarativeInterpreter(gc);
            stepStarter.SetEventConsumer(declarativeInterpreter);
            stepStarter.IsDry = true;

            // Соединяем точку входа для событий с eventConsumer задания
            lock (_syncExt) _eventConsumer = declarativeInterpreter;

            State = JobState.DryRun;
            _log.Info("[thread: {0}]  Starting dryrun of WF#{1}", Thread.CurrentThread.ManagedThreadId, this.WfId);

            try
            {
                // Синхронный вызов, запускающий долгое исполнение WF
                declarativeInterpreter.ExecuteFlow(_wfId, _parseResult.ScriptModel.MainFlow, _flowDataContext, _flowExecutionProperties);

                List<Pair<long, long>> Depends = declarativeInterpreter.GetRealDependencies();

                if (Depends.Count > 0)
                {
                    List<TaskDependency> tds = new List<TaskDependency>();
                    // convert to real ids
                    foreach (var depend in Depends)
                    {
                        var td = new TaskDependency();
                        _log.Trace("Trying to find accordance for {0} -> {1} steps", depend.First, depend.Second);
                        td.ChildTaskId = IdAccordanceDict.Instance.GetRealId(WfId, depend.First);
                        td.ParentTaskId = IdAccordanceDict.Instance.GetRealId(WfId, depend.Second);
                        td.WfId = WfId.ToString();
                        tds.Add(td);
                    }

                    //==============================================
                    // VERY BAD. MOVE TO INFRA LEVEL
                    //==============================================
                    ExecutionBrokerServiceClient cli = new ExecutionBrokerServiceClient();
                    _log.Trace("Dependencies: {0}", tds.ToJsonString());
                    cli.DefineDependenciesAsync(tds.ToArray());
                    _log.Info("Defined {0} dependencies to Execution", Depends.Count);
                }

                _log.Info("[thread: {0}]  Dryrun of WF#{1} finished. Interpreter state: {2}", Thread.CurrentThread.ManagedThreadId, this.WfId, declarativeInterpreter.State);

                // Решаем, выставлять ошибку или успех
                if (declarativeInterpreter.State == DeclarativeInterpreter.InterpreterState.error)
                {
                    _log.Trace("Changing job state to Error");
                    ErrorUserComment = declarativeInterpreter.GetErrorUserComment();
                    VerboseErrorComment = declarativeInterpreter.GetVerboseErrorComment();
                    State = JobState.Error;
                }
                else
                    State = JobState.DryRunFinished;
            }
            catch (Exception ex)
            {
                // TODO: уточнить выбрасываемые исключения и обработать их
                _log.ErrorException(String.Format("[thread: {0}]  Cought exception while interpretion of WF#{1}. CHECK MANUALLY {2} ", Thread.CurrentThread.ManagedThreadId, this.WfId, ex.Message), ex);
                ErrorUserComment = "Runtime error";
                VerboseErrorComment = "Interpretion error: " + ex.Message;
                ErrorException = ex;
                State = JobState.Error;
                // TODO: сформировать JobResult
            }
            finally
            {
            }
        }


        protected void FinalizeJob()
        {
            // TODO: think where will be results collection
            // Collect results
            _fileDescriptors = _declarativeInterpreter.Context.FileRegistry.FindFilesByType(FileDescriptor.FileType.GeneratedAfterRun);

            // TODO: съесть результат интерпретации
            _log.Info("[thread: {0}]  Interpretion of WF#{1} finished. Interpreter state: {2}", Thread.CurrentThread.ManagedThreadId, this.WfId, _declarativeInterpreter.State);

            // Решаем, выставлять ошибку или успех
            if (_declarativeInterpreter.State == DeclarativeInterpreter.InterpreterState.error)
            {
                _log.Trace("Changing job state to Error");
                ErrorUserComment = _declarativeInterpreter.GetErrorUserComment();
                VerboseErrorComment = _declarativeInterpreter.GetVerboseErrorComment();
                State = JobState.Error;
            }
            else
                State = JobState.Finished;

            _timestampFinished = DateTime.Now;

        }

        /// <summary>
        /// Процедура исполнения задания. Производит первичный запуск, затем обычный запуск.
        /// Блокирует поток обработки задания.
        /// </summary>
        protected void ExecuteJob()
        {
            // Создаем контекст
            IGlobalContext gc = new GlobalContext
                                    {
                                        FileRegistry = new DictBasedFileRegistry(),
                                        FlowSystemContext = _flowSystemContext,
                                        PackageRegistry = new ListBasedPackageRegistry(),
                                        StepStarter = _stepStarter,
                                        LongRunningController = _longRunningController,
                                        Storage = _storage
                                    };

            DeclarativeInterpreter declarativeInterpreter = new DeclarativeInterpreter(gc);
            _declarativeInterpreter = declarativeInterpreter;

            // Соединяем точку входа для событий с eventConsumer задания
            lock (_syncExt) _eventConsumer = declarativeInterpreter;

            State = JobState.Active;
            _log.Info("[thread: {0}]  Starting interpretion of WF#{1}", Thread.CurrentThread.ManagedThreadId, this.WfId);

            try
            {
                _timestampStarted = DateTime.Now;
                // Синхронный вызов, запускающий долгое исполнение WF
                declarativeInterpreter.ExecuteFlow(_wfId, _parseResult.ScriptModel.MainFlow, _flowDataContext,
                                                   _flowExecutionProperties);

                FinalizeJob();

            }
            catch (ThreadDisconnectedException tde)
            {
                throw tde;
            }
            catch (Exception ex)
            {
                // TODO: уточнить выбрасываемые исключения и обработать их
                _log.ErrorException(String.Format("[thread: {0}]  Cought exception while interpretion of WF#{1}. CHECK MANUALLY {2} ", Thread.CurrentThread.ManagedThreadId, this.WfId, ex.Message), ex);
                ErrorUserComment = "Runtime error";
                VerboseErrorComment = "Interpretion error: " + ex.Message;
                ErrorException = ex;
                State = JobState.Error;
                // TODO: сформировать JobResult
                _timestampFinished = DateTime.Now;
            }
        }

        /// <summary>
        /// Исполнение задания. Все этапы: разбор скрипта, подготовка, исполнение.
        /// Организовано в виде цикла обработки конечного автомата (перехода между состояниями).
        /// </summary>
        /// <param name="flowSystemContext"></param>
        /// <param name="stepStarter"></param>
        /// <param name="storage"></param>
        /// <param name="longRunningController"></param>
        public void Execute(FlowSystemContext flowSystemContext, IStepStarter stepStarter, IStorage storage, ILongRunningController longRunningController)
        {
            if (flowSystemContext == null || stepStarter == null)
                throw new ArgumentNullException();

            _flowSystemContext = flowSystemContext;
            _stepStarter = stepStarter;
            _storage = storage;
            _longRunningController = longRunningController;

            while (_jobState != JobState.Error && _jobState != JobState.Finished)
            {
                switch (_jobState)
                {
                    case JobState.Created:
                        try
                        {
                            ParseScript();
                        }
                        catch (Exception e)
                        {
                            _log.ErrorException(String.Format("[thread: {0}]  Error while parsing script", Thread.CurrentThread.ManagedThreadId), e);
                            ErrorUserComment = "Parsing error";
                            ErrorException = e;
                            State = JobState.Error;
                            _timestampFinished = DateTime.Now;
                            return;
                        }
                        break;
                    case JobState.Parsed:
                        try
                        {
                            ValidateModel();
                        }
                        catch (Exception e)
                        {
                            _log.ErrorException(String.Format("[thread: {0}]  Error while validating model", Thread.CurrentThread.ManagedThreadId), e);
                            ErrorUserComment = "Job validation failed";
                            ErrorException = e;
                            State = JobState.Error;
                            _timestampFinished = DateTime.Now;
                            return;
                        }
                        break;
                    case JobState.Validated:
                        try
                        {
                            DryRun();
                        }
                        catch (Exception e)
                        {
                            _log.ErrorException(String.Format("[thread: {0}]  Error while dryrunning job", Thread.CurrentThread.ManagedThreadId), e);
                            ErrorUserComment = "Runtime error";
                            ErrorException = e;
                            State = JobState.Error;
                            _timestampFinished = DateTime.Now;
                            return;
                        }
                        break;
                    case JobState.DryRunFinished:
                        try
                        {
                            ExecuteJob();
                        }
                        catch (ThreadDisconnectedException tde)
                        {
                            throw tde;
                        }
                        catch (Exception e)
                        {
                            _log.ErrorException(String.Format("[thread: {0}]  Error while executing job", Thread.CurrentThread.ManagedThreadId), e);
                            ErrorUserComment = "Runtime error";
                            ErrorException = e;
                            State = JobState.Error;
                            _timestampFinished = DateTime.Now;
                            return;
                        }
                        break;
                    // valid only if thread is reconnecting
                    case JobState.Active:
                        try
                        {
                            ThreadReConnect();
                            FinalizeJob();
                        }
                        catch (ThreadDisconnectedException tde)
                        {
                            throw tde;
                        }
                        catch (Exception e)
                        {
                            _log.ErrorException(String.Format("[thread: {0}]  Error while executing job", Thread.CurrentThread.ManagedThreadId), e);
                            ErrorUserComment = "Runtime error";
                            ErrorException = e;
                            State = JobState.Error;
                            _timestampFinished = DateTime.Now;
                            return;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Возвращает состояние задачи
        /// </summary>
        public JobState State
        {
            get { lock (_syncExt) return _jobState; }
            private set
            {
                lock (_syncExt)
                {
                    _jobState = value;
                }
            }
        }

        #region JobResult

        ///// <summary>
        ///// Returns JobResult
        ///// Thread safe
        ///// </summary>
        //public JobResult Result { get { lock (_syncExt) return ConstructResult(); } }

        //private JobResult ConstructResult()
        //{
        //    JobResult jr = null;
        //    if (State == JobState.Error || State == JobState.Finished)
        //    {
        //        jr = new JobResult(_jobRequest);

        //        jr.WfId = WfId;

        //        jr.StateInJson = GetStatus();

        //        jr.ErrorComment = ErrorUserComment;
        //        jr.VerboseErrorComment = VerboseErrorComment;
        //        jr.ErrorException = ErrorException.ToString();

        //        jr.TimestampFinished = _timestampFinished;
        //        jr.TimestampStarted = _timestampStarted;
        //        jr.TimestampPushed = _timestampPushed;

        //        jr.StateInJson = this.GetStatus();
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("Job not finished");
        //    }

        //    return jr;
        //}

        #endregion


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="jobRequest">Запрос выполнения задачи</param>
        /// <param name="wfId">Идентификатор WF</param>
        public Job(JobRequest jobRequest, Guid wfId)
        {
            _log = new WfLog(LogManager.GetCurrentClassLogger(), wfId);
            _jobRequest = jobRequest;
            _wfId = wfId;
            _timestampPushed = DateTime.Now;
        }
        /// <summary>
        /// Структура, переданная клиентом
        /// </summary>
        public JobRequest JobRequest { get { return _jobRequest; } }
        /// <summary>
        /// Идентификатор WF
        /// </summary>
        public Guid WfId { get { return _wfId; } }

        public IEventConsumer GetEventConsumer()
        {
            lock (_syncExt)
            {
                if (_jobState != JobState.Active)
                {
                    throw new InvalidOperationException(String.Format("State of Job is {0} not Active", _jobState.ToString()));
                }
                else
                {
                    if (_eventConsumer == null)
                    {
                        _log.Error("WF#{0} jobState is Active and eventConsumer is not set. Impossible - CHECK MANUALLY", _wfId);
                        throw new InvalidOperationException(String.Format("WF#{0} jobState is Active and eventConsumer is not set. Impossible - CHECK MANUALLY", _wfId));
                    }
                    else
                    {
                        return _eventConsumer;
                    }
                }
            }
        }

        /// <summary>
        /// Получение информации о WF
        /// </summary>
        /// <returns></returns>
        public JobDescription GetDescription()
        {
            JobDescription ret = new JobDescription();
            lock (_syncExt)
            {
                ret.ID = _wfId;
                ret.WfId = _wfId;
                ret.State = _jobState;

                ret.PushedAt = _timestampPushed;
                ret.StartedAt = _timestampStarted;
                ret.FinishedAt = _timestampFinished;
                ret.Timestamp = DateTime.Now;

                if (JobRequest != null)
                {
                    ret.JobRequest = JobRequest;
                }

                if (_jobState == JobState.Error)
                {
                    if (ErrorUserComment != null)
                        ret.ErrorComment = ErrorUserComment.Sanitize();
                    if (VerboseErrorComment != null)
                        ret.VerboseErrorComment = VerboseErrorComment.Sanitize();
                    if (ErrorException != null)
                        ret.ErrorException = ErrorException.ToString().Sanitize();
                }

                if (_declarativeInterpreter != null)
                {
                    ret.InterpreterState = _declarativeInterpreter.State.ToString();
                    ret.Nodes = _declarativeInterpreter.GetNodeDescriptions();
                }

                // outputs
                if (_fileDescriptors != null)
                {
                    ret.Outputs = new List<FileDescription>();

                    foreach (var fileDescriptor in _fileDescriptors)
                    {
                        ret.Outputs.Add(new FileDescription()
                        {
                            FileIdentifier = fileDescriptor.FileIdentifier,
                            NStorageId = fileDescriptor.NStorageId,
                            StorageId = fileDescriptor.StorageId,
                            Id = fileDescriptor.Id.ToString(),
                            Type = fileDescriptor.Type.ToString()
                        });
                    }

                }
            }
            return ret;

        }

        public List<LongRunningStepRunInfo> GetLongRunningRunInfos()
        {
            lock (_syncExt)
            {
                return _declarativeInterpreter.GetLongRunningRunInfos();
            }
        }

        #region Json
        //public string GetStatus()
        //{
        //    string ret = "";
        //    lock (_syncExt)
        //    {
        //        ret += "{\n";
        //        ret += "\"wfid\": '" + _wfId + "',\n";
        //        ret += "\"state\": \"" + _jobState + "\",\n";

        //        if (_timestampPushed != DateTime.MinValue) ret += "\"pushedAt\": " + JValue.FromObject(_timestampPushed).ToString() + ",\n";
        //        if (_timestampStarted != DateTime.MinValue) ret += "\"startedAt\": " + JValue.FromObject(_timestampStarted).ToString() + ",\n";
        //        if (_timestampFinished != DateTime.MinValue) ret += "\"finishedAt\": " + JValue.FromObject(_timestampFinished).ToString() + ",\n";

        //        if (JobRequest != null)
        //        {
        //            ret += "'JobRequest': {\n";

        //            ret += "'Script': " + JobRequest.Script.Sanitize() + ",\n";
        //            ret += "'ScriptDataContext': " + JobRequest.ScriptDataContext.Sanitize() + ",\n";
        //            ret += "'FlowExecutionProperties': " + JobRequest.FlowExecutionProperties.Sanitize() + "\n";

        //            ret += "},\n";
        //        }

        //        if (_jobState == JobState.Error)
        //        {
        //            if (ErrorUserComment != null) ret += "\"errorComment\": " + ErrorUserComment.Sanitize() + ",\n";
        //            if (VerboseErrorComment != null) ret += "\"verboseErrorComment\": " + VerboseErrorComment.Sanitize() + ",\n";
        //            if (ErrorException != null) ret += "\"errorException\": " + ErrorException.ToString().Sanitize() + ",\n";
        //        }

        //        if (_declarativeInterpreter != null)
        //            ret += _declarativeInterpreter.GetStatus();

        //        // outputs
        //        if (_fileDescriptors != null)
        //        {
        //            ret += "\"outputs\": [\n";

        //            foreach (var fileDescriptor in _fileDescriptors)
        //            {
        //                ret += "  {\n";
        //                ret += "  'type': 'File',";
        //                ret += "  'fileIdentifier': '" + fileDescriptor.FileIdentifier + "',";
        //                ret += "  'nStorageId': '" + fileDescriptor.NStorageId + "',";
        //                ret += "  'storageId': '" + fileDescriptor.StorageId + "',";
        //                ret += "  'internalId': '" + fileDescriptor.Id + "',";
        //                ret += "  },\n";
        //            }

        //            ret += "]\n";
        //        }


        //        ret += "}\n";
        //    }
        //    return ret;
        //}
        #endregion


        #region controlling
        public void Pause()
        {
            if (_declarativeInterpreter != null)
            {
                _log.Trace("Pausing job");
                _declarativeInterpreter.Pause();
            }
        }

        public void Resume()
        {
            if (_declarativeInterpreter != null)
            {
                _log.Trace("Resuming job");
                _declarativeInterpreter.Resume();
            }
        }

        public void Stop()
        {
            if (_declarativeInterpreter != null)
            {
                _log.Trace("Stopping job");
                _declarativeInterpreter.Stop();
            }
        }
        public void Pause(long blockId)
        {
            if (_declarativeInterpreter != null)
            {
                _log.Trace("Pausing job");
                _declarativeInterpreter.Pause(blockId);
            }
        }

        public void Resume(long blockId)
        {
            if (_declarativeInterpreter != null)
            {
                _log.Trace("Resuming job");
                _declarativeInterpreter.Resume(blockId);
            }
        }

        public void Stop(long blockId)
        {
            if (_declarativeInterpreter != null)
            {
                _log.Trace("Stopping job task");
                _declarativeInterpreter.Stop(blockId);
            }
        }
        #endregion

        #region Threading
        public void ThreadReConnect()
        {
            try
            {
                _timestampStarted = DateTime.Now;
                // Синхронный вызов, запускающий долгое исполнение WF
                _declarativeInterpreter.ThreadReConnect();

                FinalizeJob();

            }
            catch (ThreadDisconnectedException tde)
            {
                throw tde;
            }
            catch (Exception ex)
            {
                // TODO: уточнить выбрасываемые исключения и обработать их
                _log.ErrorException(String.Format("[thread: {0}]  Cought exception while interpretion of WF#{1}. CHECK MANUALLY {2} ", Thread.CurrentThread.ManagedThreadId, this.WfId, ex.Message), ex);
                ErrorUserComment = "Runtime error";
                VerboseErrorComment = "Interpretion error: " + ex.Message;
                ErrorException = ex;
                State = JobState.Error;
                // TODO: сформировать JobResult
                _timestampFinished = DateTime.Now;
            }
        }
        #endregion
    }
}