using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common.DataContracts;
using Easis.Wfs.EasyFlow.Model;
using NLog;

namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Включает информацию о состоянии
    /// обработка триггеров, вызов действий
    /// </summary>
    public abstract class NodeBase : IControllable //IJsonMonitorable
    {
        protected static readonly Logger _log = LogManager.GetCurrentClassLogger();

        #region Parsed data
        /// <summary>
        /// Объектное предтавление узла, разобранное из скрипта EasyFlow
        /// </summary>
        protected BlockBase _block;
        /// <summary>
        /// Идентификатор узла
        /// </summary>
        public long Id { get { return _block.Id; } }
        #endregion

        #region Triggers
        //TODO: переписать: не очень клево
        public class EventState
        {
            // Шаблон без wfId и без arg, FlowEvent.UNDEFINED_ID в значении id означает wildcard
            public FlowEvent EventDef { get; set; }
            public bool Fires { get; set; }
            public FlowEvent EventInstance { get; set; }
        }

        public class EventStateTable
        {
            public string ActionName;
            public TriggerEventExpressionType EventExpressionType;
            public ICollection<EventState> EventStates;
        }

        private readonly IDictionary<string, EventStateTable> _actionTriggers;
        public IDictionary<string, EventStateTable> _triggers { get { return _actionTriggers; } }

        // новые подписки. Необходимы для добавления новых подписок во время исполнения WF.
        private class Subscription
        {
            public string ActionName { get; set; }
            public FlowEvent EventDef { get; set; }
            public TriggerEventExpressionType ExprType { get; set; }
        }

        private readonly Queue<Subscription> _newSubscriptions = new Queue<Subscription>();

        /// <summary>
        /// Подписка на событие и вызов метода (данным методом нельзя создать вызов по нескольким методам).
        /// Этот метод можно вызывать внутри Node. 
        /// Он устанавливает новую подписку в очередь подписок.
        /// </summary>
        protected void Subscribe(string actionName, FlowEvent flowEvent, TriggerEventExpressionType type)
        {
            _newSubscriptions.Enqueue(new Subscription()
                                          {ActionName = actionName, EventDef = flowEvent, ExprType = type});
        }

        private void CheckSubscriptions()
        {
            while (_newSubscriptions.Count > 0)
            {
                Subscription newSubscription = _newSubscriptions.Dequeue();
                AddDependencyTrigger(newSubscription.EventDef.BlockId, newSubscription.ActionName, newSubscription.EventDef.Name, newSubscription.ExprType);
            }
        }

        /// <summary>
        /// Усваивание информации о произошедшем событии, корректировка подписок.
        /// </summary>
        /// <param name="flowEvent">Событие</param>
        /// <param name="actions">Список поддерживаемых действий</param>
        private void AssimilateEventToActions(FlowEvent flowEvent, IEnumerable<string> actions = null)
        {
            // Цикл по действиям
            foreach (var actionName in _actionTriggers.Keys)
            {
                if (actions == null || actions.Contains(actionName))
                {
                    // подошло ли хоть под одно событие?
                    bool matched = false;

                    // Цикл по событиям внутри триггера
                    foreach (var es in _actionTriggers[actionName].EventStates)
                    {
                        // Сравнение описатора события и произошедшего события - Match
                        if ((es.EventDef.BlockId == flowEvent.BlockId || es.EventDef.BlockId == FlowEvent.UNDEFINED_ID) &&
                            es.EventDef.Name == flowEvent.Name)
                        {
                            es.Fires = true;
                            es.EventInstance = flowEvent;

                            matched = true;
                        }
                    }

                    // проверяем, сработало ли событие
                    if (matched)
                    {
                        // сработал ли триггер?
                        bool fired;

                        // AND
                        if (_actionTriggers[actionName].EventExpressionType == TriggerEventExpressionType.And)
                            fired = _actionTriggers[actionName].EventStates.Aggregate(true,
                                                                                      (rslt, value) =>
                                                                                      rslt && value.Fires);
                            // OR
                        else
                            fired = _actionTriggers[actionName].EventStates.Aggregate(false,
                                                                                      (rslt, value) =>
                                                                                      rslt || value.Fires);
                        if (fired)
                        {
                            //Pause specific actions
                            // сохраняем состояние до паузы
                            if (actionName == ACTION_PAUSE)
                            {
                                _stateBeforePause = State;
                            }
                            
                            // Внимание! Опасное место.
                            // Разное поведение когда много евентов в триггере и когда один.
                            // если несколько евентов в триггере => передается list
                            if (_actionTriggers[actionName].EventStates.Count == 1)
                                Action(actionName,
                                       _actionTriggers[actionName].EventStates.First().EventInstance.EventArg);
                            else
                            {
                                if(_actionTriggers[actionName].EventExpressionType == TriggerEventExpressionType.And)
                                    // возвращаем список
                                    Action(actionName, _actionTriggers[actionName].EventStates.Select(state => state.EventInstance.EventArg));
                                else
                                    //TODO: найти сработавший триггер
                                    Action(actionName);
                            }
                                

                            //unPause specific actions
                            // Проверяем, есть ли накопленные во время ожидания на паузе события)
                            if (actionName == ACTION_RESUME)
                            {
                                AssimilatePausedEvents();
                                State = _stateBeforePause;
                            }
                        }
                    }
                }
            }
        }

        #region Pause
        private Queue<FlowEvent> _eventsPauseBuffer = new Queue<FlowEvent>();
        private string _stateBeforePause = null;

        // Проверяем, есть ли накопленные во время ожидания на паузе события
        // Сделано, чтобы обеспечить функционирование паузы
        private void AssimilatePausedEvents()
        {
            while (_eventsPauseBuffer.Count > 0)
            {
                var fe = _eventsPauseBuffer.Dequeue();
                AssimilateEventToActions(fe, new string[] { ACTION_START, ACTION_HANDLE_ERROR });
            }
        }
        #endregion

        private void AssimilateEvent(FlowEvent flowEvent)
        {
            // Проверяем, есть ли новые подписки
            CheckSubscriptions();

            // В режиме паузы пропускаем только управляющие события, остальные собираем в очередь
            if (State == STATE_PAUSED)
            {
                AssimilateEventToActions(flowEvent, new string[] { ACTION_PAUSE, ACTION_RESUME, ACTION_STOP });
                _eventsPauseBuffer.Enqueue(flowEvent);
            }
            else
            {
                //toall
                AssimilateEventToActions(flowEvent);
            }
        }

        // Вертикальные зависимости
        private readonly ICollection<NodeBase> _parents = new List<NodeBase>();
        private readonly ICollection<NodeBase> _children = new List<NodeBase>();
        /// <summary>
        /// Родительские узлы, узлы, от которых зависиз данный узел
        /// </summary>
        public ICollection<NodeBase> Parents
        {
            get { return _parents; }
        }
        /// <summary>
        /// Зависимые узлы
        /// </summary>
        public ICollection<NodeBase> Children
        {
            get { return _children; }
        }

        public void AddNodeParent(NodeBase node)
        {
            _parents.Add(node);
        }

        public void AddNodeChild(NodeBase node)
        {
            _children.Add(node);
        }

        /// <summary>
        /// Возвращает список ID узлов, от которых зависит.
        /// Во внимание принимаются только start-finished зависимости.
        /// </summary>
        /// <returns>Набор ID</returns>
        public IEnumerable<long> DependsOnNodeIndexes()
        {
            List<long> list = new List<long>();
            // проверяем только start-finished зависимости
            if (_actionTriggers.ContainsKey(ACTION_START))
                foreach (var ev in _actionTriggers[ACTION_START].EventStates)
                {
                    list.Add(ev.EventDef.BlockId);
                }
            return list;
        }

        /// <summary>
        /// Проставляет parent-child связи.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParentChildConnection(NodeBase parent)
        {
            // Child-parent связь
            AddNodeParent(parent);
            parent.AddNodeChild(this);
        }

        public void AddDependencyTrigger(long fromId, string action, string eventstring, TriggerEventExpressionType triggerEventExpressionType)
        {
            if (!_actionTriggers.ContainsKey(action))
            {
                EventStateTable table = new EventStateTable
                {
                    ActionName = action,
                    EventExpressionType = triggerEventExpressionType,
                    EventStates = new List<EventState>()
                };

                _actionTriggers.Add(action, table);
            }

            // Force change to triggerEventExpressionType
            _actionTriggers[action].EventExpressionType = triggerEventExpressionType;

            // Добавляем доп. условие срабатывания
            EventState es = new EventState
            {
                EventDef = new FlowEvent(eventstring, Guid.Empty, fromId),
                Fires = false,
                EventInstance = null
            };

            _actionTriggers[action].EventStates.Add(es);
        }

        /// <summary>
        /// Добавление зависимости между узлами
        /// </summary>
        /// <param name="from">От какого узла зависит</param>
        /// <param name="action">Действие</param>
        /// <param name="eventstring">Событие</param>
        /// <param name="triggerEventExpressionType">Логическая операция для условий подписки</param>
        public void AddDependencyTrigger(NodeBase from, string action, string eventstring, TriggerEventExpressionType triggerEventExpressionType)
        {
            AddDependencyTrigger(from.Id,action,eventstring,triggerEventExpressionType);
        }

        /// <summary>
        /// Helper для добавления триггера. 
        /// Проставляет parent-child связи.
        /// Добавляет событие в условие срабатывания start по правилу AND (устанавливает в AND).
        /// </summary>
        /// <param name="from">Узел от которого зависит данный</param>
        public void AddStartFinishDependency(NodeBase from)
        {
            AddDependencyTrigger(from, ACTION_START, FlowEvent.BLOCK_FINISHED, TriggerEventExpressionType.And);
        }

        /// <summary>
        /// Helper для добавления триггера. 
        /// </summary>
        /// <param name="from">Узел от которого зависит данный</param>
        public void AddErrorHandlingDependency(NodeBase from)
        {
            AddDependencyTrigger(from, ACTION_HANDLE_ERROR, FlowEvent.BLOCK_FINISHED_WITH_ERROR, TriggerEventExpressionType.Or);
        }

        /// <summary>
        /// Helper для добавления триггера. 
        /// </summary>
        /// <param name="from">Узел от которого зависит данный</param>
        public void AddControllingDependency(NodeBase from)
        {
            //AddDependencyTrigger(from, ACTION_PAUSE, FlowEvent.BLOCK_PAUSED, TriggerEventExpressionType.Or);
            AddDependencyTrigger(from, ACTION_STOP, FlowEvent.BLOCK_STOPPED, TriggerEventExpressionType.Or);
            //AddDependencyTrigger(from, ACTION_RESUME, FlowEvent.BLOCK_RESUMED, TriggerEventExpressionType.And);
        }

        #endregion


        #region private configurable
        /// <summary>
        /// Контекст
        /// </summary>
        protected INodeContext Context { get; set; }

        //// для генерации событий
        //private readonly IInternalEventGenerator _internalEventGenerator;
        //// необходим для анализа входных параметров и выходных, pre и post
        //protected readonly ICodeInterpreter CodeInterpreter;
        #endregion

        #region States
        // Общие состояния для всех узлов
        public const string STATE_INITIALIZE = "state_init";
        public const string STATE_STARTED = "state_started";
        public const string STATE_PAUSED = "state_paused";
        public const string STATE_STOPPED = "state_stopped";
        public const string STATE_FINISHED = "state_finished";
        public const string STATE_ERROR = "state_error";

        private string _state = STATE_INITIALIZE;
        /// <summary>
        /// Состояние узла
        /// </summary>
        public virtual string State
        {
            get { return _state; }
            protected set
            {
                _log.Debug("                         Node#{0} state changed {1} -> {2}", Id, _state, value);
                _state = value;

                // генерируем события только для стандартных FINISHED и STARTED
                switch (_state)
                {
                    case STATE_FINISHED:
                        _log.Trace("                         Node#{0} generated event BLOCK_FINISHED", Id);
                        GenerateEvent(FlowEvent.BLOCK_FINISHED);
                        break;
                    case STATE_STARTED:
                        _log.Trace("                         Node#{0} generated event BLOCK_STARTED", Id);
                        GenerateEvent(FlowEvent.BLOCK_STARTED);
                        break;
                    case STATE_PAUSED:
                        _log.Trace("                         Node#{0} generated event BLOCK_PAUSED", Id);
                        GenerateEvent(FlowEvent.BLOCK_PAUSED);
                        break;
                    case STATE_STOPPED:
                        _log.Trace("                         Node#{0} generated event BLOCK_STOPPED", Id);
                        GenerateEvent(FlowEvent.BLOCK_STOPPED);
                        break;
                    case STATE_ERROR:
                        _log.Trace("                         Node#{0} generated event BLOCK_FINISHED_WITH_ERROR", Id);
                        GenerateEvent(FlowEvent.BLOCK_FINISHED_WITH_ERROR);
                        break;
                    default:
                        break;
                }

                // обновляем представление
                UpdateStatus();
            }
        }

        public string ErrorUserComment { get; protected set; }
        public string VerboseErrorComment { get; protected set; }
        public Exception ErrorException { get; protected set; }

        #endregion

        /// <summary>
        /// from FlowGraph
        /// </summary>
        public virtual void Initialize()
        {
            State = STATE_INITIALIZE;
        }

        /// <summary>
        /// Конструктор
        /// Triggers eating
        /// </summary>
        /// <param name="block"></param>
        /// <param name="context"></param>
        protected NodeBase(BlockBase block, INodeContext context)
        {
            _block = block;
            Context = context;
            //CodeInterpreter = codeInterpreter;
            //_internalEventGenerator = internalEventGenerator;

            //TODO: eat triggers
            // Формирование таблицы состояния событий и триггеров
            // Внимание! При изменении логики работы с триггерами, даный кусок изменить.
            _actionTriggers = new Dictionary<string, EventStateTable>();
            foreach (var actionName in _block.Triggers.Keys)
            {
                EventStateTable est = new EventStateTable
                                          {
                                              EventExpressionType =
                                                  _block.Triggers[actionName].EventExpression.ExpressionType,
                                              ActionName = actionName,
                                              EventStates = new List<EventState>()
                                          };

                foreach (var evt in _block.Triggers[actionName].EventExpression.Events)
                {
                    EventState es = new EventState
                                        {
                                            EventDef = new FlowEvent(evt.Name, Guid.Empty, evt.Source.Id, null),
                                            EventInstance = null,
                                            Fires = false
                                        };

                    est.EventStates.Add(es);
                }

                _actionTriggers.Add(actionName, est);
            }
        }

        /// <summary>
        /// Для внешних клиентов
        /// </summary>
        /// <param name="flowEvent"></param>
        public void Notify(FlowEvent flowEvent)
        {
            //пропустить через триггеры изменив их состояние
            AssimilateEvent(flowEvent);
        }

        /// <summary>
        /// Для генерации события из node
        /// </summary>
        /// <param name="eventName">Наименование события</param>
        protected void GenerateEvent(string eventName)
        {
            Context.InternalEventGenerator.GenerateEvent(eventName, Id);
        }

        #region Actions
        public const string ACTION_START  = "action_start";
        public const string ACTION_PAUSE  = "action_pause";
        public const string ACTION_RESUME = "action_resume";
        public const string ACTION_STOP   = "action_stop";
        public const string ACTION_HANDLE_ERROR = "action_handle_error";

        protected abstract void Action(string actionName, object arg = null);
        #endregion

        public override string ToString()
        {
            return String.Format("Node#{0}({1})", Id, State);
        }


        private object _syncRoot = new object();

        #region Status caches
        //private string statusCache;
        private NodeDescription descriptionCache;

        /// <summary>
        /// Update status caches
        /// </summary>
        public void UpdateStatus()
        {
            lock (_syncRoot)
            {
                //statusCache = GetNodeStatus();
                descriptionCache = GetNodeDescription();
            }
        }

        //public string GetStatus()
        //{
        //    string ret = "";
        //    lock (_syncRoot)
        //    {
        //        ret = statusCache;
        //    }
        //    return ret;
        //}

        public NodeDescription GetDescription()
        {
            NodeDescription ret = null;
            lock (_syncRoot)
            {
                ret = descriptionCache;
            }
            return ret;
        }

        //public abstract string GetNodeStatus();
        public abstract NodeDescription GetNodeDescription();
        #endregion

        #region Dynamic creation
        public abstract NodeBase CloneNode(long newId, string newName, IEnumerable<NamedParameter> newParams);
        #endregion
        
        public void Pause()
        {
            Action(ACTION_PAUSE);
        }

        public void Resume()
        {
            Action(ACTION_RESUME);
        }

        public void Stop()
        {
            Action(ACTION_STOP);
        }
    }
}
