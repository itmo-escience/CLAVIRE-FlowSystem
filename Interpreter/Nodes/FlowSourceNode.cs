using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common.DataContracts;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    public class FlowSourceNode : NodeBase
    {
        public FlowSourceNode(BlockBase block, INodeContext context)
            : base(block, context)
        {
            // Создаем триггер на старт WF
            Subscribe(ACTION_START,new FlowEvent(FlowEvent.FLOW_STARTED,Guid.Empty,-1,null), TriggerEventExpressionType.Or);
        }

        protected override void Action(string actionName, object arg)
        {
            switch (actionName)
            {
                // Старт WF
                case ACTION_START:
                    State = STATE_STARTED;
                    State = STATE_FINISHED;
                    break;
                case ACTION_HANDLE_ERROR:
                    State = STATE_ERROR;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(String.Format("Unknown action {0} for FlowSourceNode", actionName));
            }
        }

        public override string ToString()
        {
            return String.Format("Start");
        }

        public override NodeBase CloneNode(long newId, string newName, IEnumerable<NamedParameter> newParams)
        {
            throw new NotImplementedException();
        }

        //public override string GetNodeStatus()
        //{
        //    // stub
        //    return "";
        //}

        public override NodeDescription GetNodeDescription()
        {
            return null;
        }
    }
}
