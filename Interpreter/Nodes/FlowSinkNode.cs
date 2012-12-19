using System;
using System.Collections.Generic;
using Easis.Common.DataContracts;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.Interpreting
{
    public class FlowSinkNode : NodeBase
    {
        //private readonly DeclarativeInterpreter _declarativeInterpreterCopy;

        protected new ISinkNodeContext Context { get; set; }

        public FlowSinkNode(BlockBase block, ISinkNodeContext context, WfLog log) : base(block, context, log)
        {
            Context = context;
            
            // явно сохраняем
            //_declarativeInterpreterCopy = declarativeInterpreter;
        }

        protected override void Action(string actionName, object arg = null)
        {
            switch (actionName)
            {
                // Завершение WF
                case ACTION_START:
                    State = STATE_STARTED;
                    _log.Trace("SinkNode finishes execution");
                    Context.DeclarativeInterpreter.Finish();
                    State = STATE_FINISHED;
                    break;
                case ACTION_HANDLE_ERROR:
                    State = STATE_ERROR;
                    Context.DeclarativeInterpreter.FinishWithError();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(String.Format("Unknown action {0} for FlowSinkNode", actionName));
            }
        }

        public override string ToString()
        {
            return String.Format("Finish");
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
