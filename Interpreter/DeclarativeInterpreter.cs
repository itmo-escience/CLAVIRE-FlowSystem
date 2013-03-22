using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Easis.Common;
using Easis.Common.DataContracts;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.Interpreting.Nodes;
using Easis.Wfs.Interpreting.Utils;
using NLog;
using ExecutionContext = Easis.Common.DataContracts.ExecutionContext;

namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Интерпретатор декларативного кода. Отвечает за исполение одного WF на абстрактной инфраструктуре. Механизм работы построен согласно событийной модели.
    /// Поддерживает многопоточный доступ.
    /// </summary>
    public class DeclarativeInterpreter : IEventConsumer, IInternalEventGenerator, IControllable, IController, IThreadConnectable //IJsonMonitorable
    {
        WfLog _log = null;

        private object _syncRoot = new object();

        #region runtime

        // Контекст данных доступный посредством 'транзакций'
        /// <summary>
        /// Глобальная область видимости. Память интерпретатора.
        /// </summary>
        protected GlobalDataScope GlobalDataScope;

        // WF
        private FlowGraph _flowGraph = null;

        private BlockBase _sourceBlock;
        private BlockBase _sinkBlock;

        // Уберконтекст
        public IGlobalContext Context;

        #endregion

        #region configurable private data

        private Guid _wfId;
        private Flow _flow;

        private ExecutionContext _flowExecutionProperties;
        private FlowDataContext _flowDataContext;

        #endregion

        #region State

        // Состояние останавливает обработку событий
        public enum InterpreterState
        {
            active,
            paused,
            stopped,
            finished,
            error
        }

        private InterpreterState _state = InterpreterState.active;

        public InterpreterState State
        {
            get { lock (_syncRoot) return _state; }
            protected set { lock (_syncRoot) _state = value; }
        }

        /// <summary>
        /// Завершение работы интерпретатора.
        /// Вызывается из FlowSinkNodeClass
        /// </summary>
        public void Finish()
        {
            State = InterpreterState.finished;
            _log.Debug("Interpreter state changed to finished");
        }

        /// <summary>
        /// Внешний метод. Приостановка работы интерпретатора.
        /// </summary>
        public void Pause()
        {
            switch (State)
            {
                case InterpreterState.paused:
                    _log.Error("Can't pause interpreter which have already been paused. Ignorring.");
                    break;
                case InterpreterState.active:
                    State = InterpreterState.paused;
                    PushEvent(new FlowEvent(FlowEvent.CONTROL_FLOW_PAUSE, WfId, FlowEvent.UNDEFINED_ID));
                    break;
                default:
                    _log.Error("Can't pause interpreter which was finished. Ignorring.");
                    break;
            }
        }

        /// <summary>
        /// Внешний метод. Возобновление работы интерпретатора.
        /// </summary>
        public void Resume()
        {
            switch (State)
            {
                case InterpreterState.paused:
                    State = InterpreterState.active;
                    PushEvent(new FlowEvent(FlowEvent.CONTROL_FLOW_RESUME, WfId, FlowEvent.UNDEFINED_ID));
                    break;
                case InterpreterState.active:
                    _log.Error("Can't resume interpreter which was not paused. Ignorring.");
                    break;
                default:
                    _log.Error("Can't resume interpreter which was not paused. Error.");
                    FinishWithError();
                    break;
            }
        }

        /// <summary>
        /// Внешний метод. Остановка работы интерпретатора.
        /// </summary>
        public void Stop()
        {
            switch (State)
            {
                case InterpreterState.active:
                    State = InterpreterState.stopped;
                    PushEvent(new FlowEvent(FlowEvent.CONTROL_FLOW_STOP, WfId, FlowEvent.UNDEFINED_ID));
                    break;
                case InterpreterState.paused:
                    State = InterpreterState.stopped;
                    PushEvent(new FlowEvent(FlowEvent.CONTROL_FLOW_STOP, WfId, FlowEvent.UNDEFINED_ID));
                    break;
                default:
                    _log.Error("Couldn't stop interpreter which had already been stopped. Ignorring.");
                    FinishWithError();
                    break;
            }
        }

        //TODO: Сделать структуру данных об ошибке, заполняемую отлавливающей стороной
        /// <summary>
        /// Остановка интерпретатора с ошибкой.
        /// Вызывается из FlowSinkNode
        /// </summary>
        public void FinishWithError()
        {
            //TODO: Останавливаем все активные шаги

            State = InterpreterState.error;
            _log.Debug("Interpreter state changed to error");
        }

        #endregion

        #region Events
        // Внутренняя шина событий.
        // event queue / thread safe
        private readonly SyncronizedBlockingQueue<FlowEvent> _eventBus = new SyncronizedBlockingQueue<FlowEvent>();

        /// <summary>
        /// Внешний метод. Добавление нового события в очередь событийной шины.
        /// </summary>
        /// <param name="flowEvent"></param>
        public void PushEvent(FlowEvent flowEvent)
        {
            lock (_syncRoot)
            {
                _eventBus.Enqueue(flowEvent);
                _log.Info("External event enqueued {0}", flowEvent.ToString());
            }
        }

        /// <summary>
        /// Генерация нового события.
        /// Внутренний метод (для узлов, наследников NodeBase)
        /// </summary>
        /// <param name="eventName">Название события</param>
        /// <param name="sourceBlockId">Идентификатор узла</param>
        public void GenerateEvent(string eventName, long sourceBlockId = FlowEvent.UNDEFINED_ID)
        {
            FlowEvent flowEvent = new FlowEvent(eventName, _wfId, sourceBlockId);
            _eventBus.Enqueue(flowEvent);
            _log.Debug("Internal event enqueued {0}", flowEvent.ToString());
        }
        #endregion

        #region Stages of work

        /// <summary>
        /// Регистрирация входных данных в памяти интерпретатора, как глобальных переменных.
        /// </summary>
        private void SetupDataContext()
        {
            _log.Trace("Setup data context for {0} input files", _flowDataContext.InputFiles.Count);
            foreach (var f in _flowDataContext.InputFiles)
            {
                // magic data context
                var strs = f.Value.Split(new char[] { '|' });

                FileDescriptor fileDescriptor = Context.FileRegistry.CreateFileDescriptor();
                fileDescriptor.Type = FileDescriptor.FileType.Required;

                // for compatibility with old starters without | 
                if (strs.Length < 2)
                    fileDescriptor.FileIdentifier = strs[0];
                else
                    fileDescriptor.FileIdentifier = strs[1];

                fileDescriptor.NStorageId = strs[0];

                // регистрируем глобальные переменные
                Variable v = new Variable(f.Key, new FileValue(fileDescriptor));
                GlobalDataScope.Variables.Add(v);
                _log.Trace("Created variable {0} for input file {1}", v.Name, f.Value);
            }
        }

        /// <summary>
        /// Создание FlowGraph из Flow (объектное представление WF, полученное от компонента разбора EasyFlow).
        /// Создает _flowGraph.
        /// </summary>
        private void CreateFlowGraph()
        {
            ICollection<NodeBase> nodes = new List<NodeBase>();

            _flowGraph = new FlowGraph(WfId);

            foreach (var block in _flow.Blocks)
            {
                if (!block.IsDisabled)
                {
                    if (block is StepBlock)
                    {
                        _log.Debug("StepBlock#{0} recognized", block.Id);

                        // Формируем контекст
                        ICodeInterpreter codeInterpreter = new ImperativeInterpreter(GlobalDataScope,
                                                                                     ((StepBlock)block).Name);

                        // looking for sweeping
                        bool isSweeper = false;
                        foreach (var parameter in ((StepBlock)block).RunParameters.Parameters)
                        {
                            if (parameter.IsSweepParam)
                            {
                                isSweeper = true;
                                break;
                            }
                        }

                        StepNode stepNode = null;

                        if (((StepBlock)block).IsLongRunning)
                        {
                            ILongRunningStepNodeContext snc = Context.CreateLongRunningStepNodeContext(WfId, codeInterpreter, this, _flowGraph);
                            snc.ExecutionContext = _flowExecutionProperties;
                            stepNode = new LongRunningStepNode((StepBlock)block, snc, _log);
                        }
                        else
                        {
                            IStepNodeContext snc = Context.CreateStepNodeContext(WfId, codeInterpreter, this, _flowGraph);
                            snc.ExecutionContext = _flowExecutionProperties;
                            if (isSweeper)
                            {
                                stepNode = new SweepStepNode((StepBlock)block, snc, _log);
                            }
                            else
                            {
                                stepNode = new StepNode((StepBlock)block, snc, _log);
                            }
                        }

                        nodes.Add(stepNode);
                    }
                    else if (block is IfBlock)
                    {
                        _log.Debug("IfBlock#{0} recognized", block.Id);
                        throw new NotImplementedException();
                    }
                    else if (block is ForBlock)
                    {
                        _log.Debug("ForBlock#{0} recognized", block.Id);
                        throw new NotImplementedException();
                    }
                    else
                    {
                        _log.Error("Unknown block type {0}. Exception ArgumentOutOfRangeException thrown.",
                                   block.GetType().ToString());
                        throw new ArgumentOutOfRangeException(String.Format("Unknown block type {0}", block.GetType()));
                    }
                }
                else
                {
                    _log.Trace("Ignoring disabled block {0} with type {1}", block.Id, block.GetType().Name);
                }
            }

            _flowGraph.AddNodes(nodes);

        }

        #region thread_swithing

        private bool _isThreadConnected = true;

        public bool IsThreadConnected
        {
            get { return _isThreadConnected; }
            set { _isThreadConnected = value; }
        }

        // thread swithing
        protected void ThreadDisconnect()
        {
            _log.Trace("WF:{0} Thread disconnected", WfId);
            IsThreadConnected = false;
        }

        // thread swithing
        /// <summary>
        /// throws ThreadDisconnectedException
        /// </summary>
        /// <returns>finished or delayed</returns>
        public void ThreadReConnect()
        {
            _log.Trace("WF:{0} Thread connected", WfId);
            IsThreadConnected = true;

            // throws ThreadDisconnectedException
            EventLoop();

            _log.Info("Cleaning ");
            CollectGarbage();
        }

        #endregion

        /// <summary>
        /// Цикл обработки событий. В цикле извлекается новое событие из очереди и передается в FlowGraph через вызов Notify.
        /// throws ThreadDisconnectedException
        /// <returns>Was loop finished by time (false) or by finishing of process (true)</returns>
        /// </summary>
        protected void EventLoop()
        {
            while (State != InterpreterState.error && State != InterpreterState.finished && State != InterpreterState.stopped)
            {
                if (State == InterpreterState.paused)
                {
                    _log.Trace("Paused, waiting for 2 sec");
                    Thread.Sleep(2000);
                }
                else
                {
                    // blocking
                    FlowEvent flowEvent = null;
                    // thread swithing
                    try
                    {
                        flowEvent = _eventBus.Dequeue(Context.ThreadWaitForEventTimeout);
                    }
                    catch (TimeoutException)
                    {
                        _log.Trace("Thread has been waiting for {0} for event in queue. Releasing.", Context.ThreadWaitForEventTimeout);
                        throw new ThreadDisconnectedException();
                    }

                    // control logic is here
                    if (flowEvent.Name.StartsWith("control_"))
                    {
                        //TODO: validate index
                        switch (flowEvent.Name)
                        {
                            case FlowEvent.CONTROL_FLOW_PAUSE:
                            case FlowEvent.CONTROL_FLOW_RESUME:
                            case FlowEvent.CONTROL_FLOW_STOP:
                                // ignoreб just for awakening
                                break;
                            case FlowEvent.CONTROL_BLOCK_PAUSE:
                                _flowGraph.GetNodeByIndex(flowEvent.BlockId).Pause();
                                break;
                            case FlowEvent.CONTROL_BLOCK_RESUME:
                                _flowGraph.GetNodeByIndex(flowEvent.BlockId).Resume();
                                break;
                            case FlowEvent.CONTROL_BLOCK_STOP:
                                _flowGraph.GetNodeByIndex(flowEvent.BlockId).Stop();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("Control event name is not supported in interpreter. See EventLoop.");
                        }
                    }
                    else
                    {
                        _flowGraph.Notify(flowEvent);
                    }
                }
            }
        }

        private void CollectGarbage()
        {
            //TODO: Удаление ненужных файлов

            _flow.Blocks.Remove(_sourceBlock);
            _flow.Blocks.Remove(_sinkBlock);
        }

        #endregion

        #region public interfaces

        public DeclarativeInterpreter(IGlobalContext context)
        {
            Context = context;
            GlobalDataScope = new GlobalDataScope();
        }
        /// <summary>
        /// Идентификатор WF
        /// </summary>
        public Guid WfId { get { return _wfId; } }

        /// <summary>
        /// Извлечение параметров исполнения из атрибутов, указанных в скрипте EasyFlow для WF.
        /// </summary>
        /// <param name="flow">Объектное представление разобранного описания WF на EasyFlow</param>
        /// <returns>Извлеченные параметры исполнения</returns>
        private IDictionary<string, string> ExtractExecutionParametersFromFlow(Flow flow)
        {
            IDictionary<string, string> ret = new Dictionary<string, string>();
            foreach (var attribute in flow.Attributes)
            {
                var val = attribute.Value.Evaluate(null).Value;
                if (val is StringValue)
                    ret.Add(attribute.Name, ((StringValue)val).AsString);
                else if (val is String)
                    ret.Add(attribute.Name, (String)val);
                else if (val is Int32)
                    ret.Add(attribute.Name, ((Int32)val).ToString());
                else
                {
                    _log.Error("Execution parameter '{0}' has unexpected type '{1}'. Ignoring.", attribute.Name, val.GetType().Name);
                }
            }
            return ret;
        }

        /// <summary>
        /// Исполнение WF. Основной метод, содержит всю логику подготовки и исполнения WF.
        /// Синхронный метод: поток блокируется до завершения WF.
        /// throws ThreadDisconnectedException
        /// </summary>
        /// <param name="wfId">Идентификатор WF</param>
        /// <param name="flow">Внутреннее представление скрипта WF</param>
        /// <param name="flowDataContext">Контекст данных пользователя.</param>
        /// <param name="flowExecutionProperties">Опции исполнения WF</param>
        public void ExecuteFlow(Guid wfId, Flow flow, FlowDataContext flowDataContext, ExecutionContext flowExecutionProperties)
        {
            _log = new WfLog(LogManager.GetCurrentClassLogger(), wfId);

            _wfId = wfId;
            _flow = flow;
            _flowDataContext = flowDataContext;
            _flowExecutionProperties = flowExecutionProperties;

            _log.Info("Initializing interpreter for WF#{0}", wfId);

            _log.Info("Extracting execution params for WF#{0}", wfId);
            var pardict = ExtractExecutionParametersFromFlow(flow);
            foreach (var kv in pardict)
            {
                _log.Trace("Flow param detected {0} {1}", kv.Key, kv.Value);
                // TODO: durty hack
                if (kv.Key.ToLower() == "priority")
                    _flowExecutionProperties.Priority = kv.Value;
                else
                    _flowExecutionProperties.ExtraElements[kv.Key] = kv.Value;

            }

            // генерация графовой структуры включающей состояния
            _log.Info("Creating FlowGraph");
            CreateFlowGraph();

            _log.Info("Setting up dependencies in graph");
            try
            {
                _flowGraph.SetupDependencies();
            }
            catch (Exception ex)
            {
                _log.Fatal("Graph consistency error. Couldn't set up dependencies. Interpreter initialization finished with error.");
                FinishWithError();
                return;
            }

            _log.Info("Validation of FlowGraph");
            _flowGraph.Validate();

            _log.Info("Creating Sink node and Source node");
            // Creating Source and Sink nodes
            // Заглушки
            _sourceBlock = new StepBlock();
            _sinkBlock = new StepBlock();
            // Получаем ID
            _flow.Blocks.Add(_sourceBlock);
            _flow.Blocks.Add(_sinkBlock);

            INodeContext nodeContext = new NodeContext()
                                           {
                                               CodeInterpreter = null, //CodeInterpreter = new ImperativeInterpreter(GlobalDataScope),
                                               InternalEventGenerator = this,
                                               NodeGraphController = _flowGraph
                                           };
            // в конструкторе проставится триггер на FLOW_START
            FlowSourceNode sourceNode = new FlowSourceNode(_sourceBlock, nodeContext, _log);

            ISinkNodeContext sinkNodeContext = new SinkNodeContext()
                                                   {
                                                       CodeInterpreter = null, //CodeInterpreter = new ImperativeInterpreter(GlobalDataScope),
                                                       InternalEventGenerator = this,
                                                       DeclarativeInterpreter = this,
                                                       NodeGraphController = _flowGraph
                                                   };
            FlowSinkNode sinkNode = new FlowSinkNode(_sinkBlock, sinkNodeContext, _log);

            _log.Info("Linking Sink and Source nodes");
            _flowGraph.InitSinkSource(sinkNode, sourceNode);

            _log.Info("Trigger validation");
            _flowGraph.ValidateTriggers();

            // 2. подготовка входных данных из FlowDataContext перенолс в scope
            SetupDataContext();

            // 5. генерация @flowstarted
            GenerateEvent(FlowEvent.FLOW_STARTED);

            // 6. set initial state for each node
            _flowGraph.InitializeNodes();

            // 7. eventloop
            _log.Info("Starting event loop");

            // thread switching ability on
            ThreadReConnect();
        }

        #endregion

        /// <summary>
        /// in external context
        /// </summary>
        /// <param name="blockId"></param>
        public void Pause(long blockId)
        {
            PushEvent(new FlowEvent(FlowEvent.CONTROL_BLOCK_PAUSE, _wfId, blockId));
        }

        /// <summary>
        /// in external context
        /// </summary>
        /// <param name="blockId"></param>
        public void Resume(long blockId)
        {
            PushEvent(new FlowEvent(FlowEvent.CONTROL_BLOCK_RESUME, _wfId, blockId));
        }

        /// <summary>
        /// in external context
        /// </summary>
        /// <param name="blockId"></param>
        public void Stop(long blockId)
        {
            PushEvent(new FlowEvent(FlowEvent.CONTROL_BLOCK_STOP, _wfId, blockId));
        }

        /// <summary>
        /// returns the cause of error
        /// </summary>
        /// <returns></returns>
        public string GetErrorUserComment()
        {
            return _flowGraph.GetErrorDescription();
        }

        public string GetVerboseErrorComment()
        {
            return _flowGraph.GetVerboseErrorDescription();
        }

        public IList<NodeDescription> GetNodeDescriptions()
        {
            return _flowGraph.GetNodeDescriptions();
        }

        public List<LongRunningStepRunInfo> GetLongRunningRunInfos()
        {
            return _flowGraph.GetLongRunningNodes().Select(node => node.LongRunInfo).ToList();
        }

        /// <summary>
        /// Return pairs of depends Child -> Parent only with real steps which will run on infrastructure
        /// </summary>
        /// <returns></returns>
        public List<Pair<long, long>> GetRealDependencies()
        {
            return _flowGraph.GetRealDependencies();
        }
    }

    public interface IInternalEventGenerator
    {
        void GenerateEvent(string eventName, long sourceBlockId = FlowEvent.UNDEFINED_ID);
    }
}
