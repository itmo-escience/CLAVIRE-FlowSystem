using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.Interpreting;
using NLog;

namespace InterpreterTestDriver
{
    class StepStarterSimulator : IStepStarter
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private IEventConsumer _eventConsumer;

        public StepStarterSimulator()
        {
            _eventConsumer = null;
        }

        public void SetEventConsumer(IEventConsumer eventConsumer)
        {
            _eventConsumer = eventConsumer;
        }

        public void StartStep(StepRunDescriptor stepRunDescriptor)
        {
            _log.Debug("StartStep invokation StepRunDescriptor:{0}",stepRunDescriptor);

            StepRunResult result = new StepRunResult();
            FlowEvent flowEvent = new FlowEvent(
                FlowEvent.RUN_FINISHED,
                stepRunDescriptor.WfId,
                stepRunDescriptor.StepId,
                result);

            _eventConsumer.PushEvent(flowEvent);
            _log.Debug("Run finished event generated FlowEvent:{0}", flowEvent);
        }

        public bool IsDry
        {
            get { return false; }
            set { throw new NotImplementedException(); }
        }
    }
}
