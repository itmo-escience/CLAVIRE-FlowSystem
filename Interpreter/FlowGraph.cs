#undef QuickGraph
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common;
using Easis.Common.DataContracts;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.Interpreting.Nodes;
using Easis.Wfs.Interpreting.Utils;
using NLog;

#if QuickGraph
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

#endif

namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Графовая структура WF, построенная по зависимостям между блоками. Необходима для анализа циклов, выполнимости, поиска истоков и стоков. Также используется для передачи событий узлам.
    /// </summary>
    public class FlowGraph : INodeGraphController //IJsonMonitorable
    {
        private readonly Guid _wfId;
        WfLog _log;

        private readonly ICollection<NodeBase> _nodes = new List<NodeBase>();
        private readonly ICollection<NodeBase> _nodesAdditions = new List<NodeBase>();

        public FlowGraph(Guid WfId)
        {
            _wfId = WfId;
            _log = new WfLog(LogManager.GetCurrentClassLogger(), WfId);
        }

        public FlowGraph(Guid WfId, ICollection<NodeBase> nodes)
        {
            _nodes = nodes;
            _log = new WfLog(LogManager.GetCurrentClassLogger(), WfId);
        }

        private FlowSinkNode _sinkNode = null;
        private FlowSourceNode _sourceNode = null;

        /// <summary>
        /// Проверка корректности графа WF (на цикличность, на выполнимость)
        /// </summary>
        public void Validate()
        {
            // TODO: реализовать 
            // empty graph
            if (_nodes.Count == 0)
            {
                throw new Exception("Workflow graph cannot be empty");
            }

            // cycles
            bool[] visited = new bool[_nodes.Count];
        }

        /// <summary>
        /// Временный метод для распечатки состояния триггеров
        /// </summary>
        private void LogTriggers()
        {
            string log = "";
            foreach (var nodeBase in _nodes)
            {
                log += "= " + nodeBase.Id + " =\n";
                foreach (var t in nodeBase._triggers)
                {
                    string evts = "";
                    foreach (var eventState in t.Value.EventStates)
                    {
                        evts += String.Format("[{0} {1}]", eventState.EventDef.BlockId, eventState.EventDef.Name);
                    }

                    log += String.Format("  - {0} {1}\n", t.Key, evts);
                }
            }
            _log.Trace("Triggers:\n{0}", log);

        }

        /// <summary>
        /// Проверка триггеров.
        /// Наличие start триггера у всех узлов.
        /// </summary>
        public void ValidateTriggers()
        {
            // TODO: реализовать 
        }

        /// <summary>
        /// Передача события для проверки подписок всем узлам. Предварительно проверяет наличие новых подписок.
        /// </summary>
        /// <param name="flowEvent">Событие</param>
        public void Notify(FlowEvent flowEvent)
        {
            CheckNewNodes();

            // brute force
            foreach (var node in _nodes)
            {
                node.Notify(flowEvent);
            }
        }

        public NodeBase GetNodeByIndex(long index)
        {
            NodeBase nb = null;

            try
            {
                nb = _nodes.First(x => x.Id == index);
            }
            catch (InvalidOperationException ex) // 
            {
                _log.Error("Node tree consistency error. Queried id {0} is absent. Exception was rethrown.");
                throw ex;
            }

            return nb;
        }

        /// <summary>
        /// Поиск стоков и истоков и связь их с SinkNode и SourceNode. Вызывается после создания истока и стока.
        /// </summary>
        public void InitSinkSource(FlowSinkNode flowSinkNode, FlowSourceNode flowSourceNode)
        {
            _sinkNode = flowSinkNode;
            _sourceNode = flowSourceNode;

            foreach (var node in _nodes)
            {
                // Ищем стоки
                if (node.Children.Count == 0)
                {
                    _sinkNode.AddStartFinishDependency(node);
                    _sinkNode.SetParentChildConnection(node);
                    _sinkNode.AddErrorHandlingDependency(node);
                }
                // Ищем истоки
                if (node.Parents.Count == 0)
                {
                    node.AddStartFinishDependency(_sourceNode);
                    node.SetParentChildConnection(_sourceNode);
                    node.AddErrorHandlingDependency(_sourceNode);
                }
            }

            // Когда стоки и истоки найдены и связи проставлены - можно добавлять sink source в список
            _nodes.Add(_sinkNode);
            _nodes.Add(_sourceNode);
        }

        /// <summary>
        /// Установка зависимостей между блоками, необходима для анализа структуры графа.
        /// На данном этапе созданы ВСЕ узлы (Nodes) кроме истокового и стокового.
        /// </summary>
        public void SetupDependencies()
        {
            foreach (var node in _nodes)
            {
                foreach (var index in node.DependsOnNodeIndexes())
                {
                    NodeBase parent = GetNodeByIndex(index); // may be exception. Logged

                    node.SetParentChildConnection(parent);
                    node.AddErrorHandlingDependency(parent);

                }
            }
        }

        ///// <summary>
        ///// Отрисовка графа.
        ///// Вызывать после SetupDependencies
        ///// </summary>
        //public string GetStatus()
        //{
        //    string ret = "";
        //    ret += "\"nodes\":[\n";
        //    bool first = true;
        //    foreach (var node in _nodes)
        //    {
        //        if (!(node is FlowSinkNode) && !(node is FlowSourceNode))
        //        {
        //            if (!first)
        //                ret += ",";
        //            else
        //                first = false;
        //            ret += node.GetStatus();
        //        }
        //    }

        //    ret += "],\n";
        //    return ret;
        //}

        /// <summary>
        /// Инициализация всех узлов в графе.
        /// invoke initialize method in fully configured nodes
        /// </summary>
        public void InitializeNodes()
        {
            foreach (var nodeBase in _nodes)
            {
                nodeBase.Initialize();
            }
        }
        /// <summary>
        /// Получение информации об ошибке за счет агрегации информации со всех узлов.
        /// </summary>
        /// <returns>Строковое представление информаци об ошибке</returns>
        public string GetErrorDescription()
        {
            string ret = "";

            foreach (var nodeBase in _nodes)
            {
                if (nodeBase.State == NodeBase.STATE_ERROR)
                {
                    if (nodeBase is StepNode)
                    {
                        ret += String.Format("{0}: {1}\r\n", ((StepNode)nodeBase).Name, nodeBase.ErrorUserComment);
                    }
                }
            }

            return ret;
        }

        public string GetVerboseErrorDescription()
        {
            string ret = "";

            foreach (var nodeBase in _nodes)
            {
                if (nodeBase.State == NodeBase.STATE_ERROR)
                {
                    if (nodeBase is StepNode)
                    {
                        ret += String.Format("{0}: {1}\r\n", ((StepNode)nodeBase).Name, nodeBase.VerboseErrorComment);
                    }
                }
            }

            return ret;
        }
        /// <summary>
        /// Проверка наличия новых узлов, созданных во время работы интерпретатора.
        /// </summary>
        private void CheckNewNodes()
        {
            if (_nodesAdditions.Count > 0)
            {
                foreach (var nodesAddition in _nodesAdditions)
                {
                    _nodes.Add(nodesAddition);
                }
                _nodesAdditions.Clear();
            }
        }

        /// <summary>
        /// Отложенное добавление узлов для вызова из узлов. Новые узлы добавляются при вызове notify.
        /// </summary>
        /// <param name="nodeBase"></param>
        public void AddNodeDeffered(NodeBase nodeBase)
        {
            _nodesAdditions.Add(nodeBase);
        }
        /// <summary>
        /// Поиск узла по имени.
        /// </summary>
        /// <param name="name">Имя узла</param>
        /// <returns>Узел</returns>
        public StepNode GetStepNodeByName(string name)
        {
            foreach (var node in _nodes)
            {
                if (node is StepNode)
                {
                    StepNode snode = node as StepNode;
                    if (snode.Name == name)
                    {
                        return snode;
                    }
                }
            }
            throw new ObjectNotFoundException(String.Format("There is no node with name '{0}'", name));
        }

        public NodeBase GetNodeById(long id)
        {
            NodeBase ret = _nodes.Where(@base => @base.Id == id).First();
            return ret;
        }

        public LongRunningStepNode[] GetLongRunningNodes()
        {
            IList<LongRunningStepNode> ret = new List<LongRunningStepNode>();
            foreach (var node in _nodes)
            {
                if (node is LongRunningStepNode)
                {
                    ret.Add(node as LongRunningStepNode);
                }
            }
            return ret.ToArray();
        }

        /// <summary>
        /// Синхронное добавление узлов
        /// </summary>
        /// <param name="nodes">Список узлов для добавления</param>
        public void AddNodes(IEnumerable<NodeBase> nodes)
        {
            foreach (var nodeBase in nodes)
            {
                _nodes.Add(nodeBase);
            }
        }

        public IList<NodeDescription> GetNodeDescriptions()
        {
            IList<NodeDescription> ret = new List<NodeDescription>();
            foreach (var node in _nodes)
            {
                if (!(node is FlowSinkNode || node is FlowSourceNode))
                    ret.Add(node.GetDescription());
            }
            return ret;
        }


        private class TempNode
        {
            public List<TempNode> Parents = new List<TempNode>();
            public List<TempNode> Children = new List<TempNode>();
            public long Id;

            public TempNode(long id)
            {
                Id = id;
            }
        }

        /// <summary>
        /// Метод получения реальной структуры WF из запусков пакетов, без динамических узлов. 
        /// Плохой метод
        /// уничтожает начальную структуру
        /// TODO: refactor!
        /// </summary>
        /// <returns>Список зависимотей</returns>
        public List<Pair<long, long>> GetRealDependencies()
        {
            List<Pair<long, long>> ret = new List<Pair<long, long>>();

            // Разворачиваем sweeps
            foreach (var node in _nodes)
            {
                if (node is SweepStepNode)
                {
                    var ssn = node as SweepStepNode;

                    foreach (var sweepChild in ssn.SweepChildren)
                    {
                        sweepChild.Parents.Clear();
                        foreach (var nodeBase in ssn.Parents)
                        {
                            sweepChild.Parents.Add(nodeBase);
                            nodeBase.Children.Remove(ssn);
                            nodeBase.Children.Add(sweepChild);
                        }

                        sweepChild.Children.Clear();
                        foreach (var nodeBase in ssn.Children)
                        {
                            if (!ssn.SweepChildren.Contains(nodeBase))
                            {
                                sweepChild.Children.Add(nodeBase);
                                nodeBase.Parents.Remove(ssn);
                                nodeBase.Parents.Add(sweepChild);
                            }
                        }

                    }

                    ssn.Parents.Clear();
                    ssn.Children.Clear();
                }
                else if (node is FlowSinkNode)
                {
                    foreach (var nodeBase in node.Parents)
                    {
                        nodeBase.Children.Remove(node);
                    }
                    node.Parents.Clear();
                }
                else if (node is FlowSourceNode)
                {
                    foreach (var nodeBase in node.Children)
                    {
                        nodeBase.Parents.Remove(node);
                    }
                    node.Children.Clear();
                }
            }

            foreach (var nodeBase in _nodes)
            {
                if (nodeBase is SweepStepNode || nodeBase is FlowSinkNode || nodeBase is FlowSourceNode)
                {
                    // skip
                }
                else
                {
                    // берем только родительские связи
                    foreach (var p in nodeBase.Parents)
                    {
                        if (ret.Where(pair => pair.First == nodeBase.Id && pair.Second == p.Id).Count() == 0)
                        {
                            ret.Add(new Pair<long, long>(nodeBase.Id, p.Id));
                            _log.Trace("Add dependency '{0} {1}' -> '{2} {3}'", nodeBase.Id, nodeBase.GetType(), p.Id,
                                       p.GetType());
                        }
                    }
                }
            }

            return ret.ToList();
        }
    }
}
