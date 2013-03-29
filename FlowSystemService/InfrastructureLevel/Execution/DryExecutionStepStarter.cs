using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Easis.Common;
using Easis.Wfs.FlowSystemService.ExecutionService;
using Easis.Wfs.Interpreting;
using NLog;

namespace Easis.Wfs.FlowSystemService.InfrastructureLevel.Execution
{
    public class DryExecutionStepStarter : ExecutionStepStarter
    {
        public override bool IsDry
        {
            get { return true; }
        }

        public DryExecutionStepStarter()
        {

        }

        private IEventConsumer _eventConsumer;

        public override void SetEventConsumer(IEventConsumer eventConsumer)
        {
            _eventConsumer = eventConsumer;
        }

        public override void StartStep(StepRunDescriptor stepRunDescriptor)
        {
            WfLog _log = new WfLog(LogManager.GetCurrentClassLogger(), stepRunDescriptor.WfId);

            ExecutionBrokerServiceClient ec = new ExecutionBrokerServiceClient();

            bool isIdNew = true;
            // 1. new id
            ulong tid;
            tid = ec.GetNewTaskId();

            TaskDescription td = FormTaskDescription(stepRunDescriptor);
            td.TaskId = tid;
            td.WfId = stepRunDescriptor.WfId.ToString();

            _log.Trace("Defining step using Execution.");
            _log.Trace(td.ToJsonString());

            // 2. define
            //ec.Define(new TaskDescription[] { td }, null);
            ec.DefineTask(td);

            // Важно соответствие ставить до вызова запускатора
            // иначе обработчик события срабатывает раньше и необходимого ключа не находится
            lock (_syncRoot)
            {
                _log.Trace("Adding triplet ({0}, {1}, {2}) to Id dict", stepRunDescriptor.WfId,
                           stepRunDescriptor.StepId, tid);
                IdAccordanceDict.Instance.AddIdAccordance(stepRunDescriptor.WfId, stepRunDescriptor.StepId, tid);
            }


            _log.Trace("Set dryrun info for step '{0}'", stepRunDescriptor.StepId);

            StepRunInfo info;

            if (stepRunDescriptor.IsLongRunning)
                info = new LongRunningStepRunInfo();
            else
                info = new StepRunInfo();

            info.ExternalId = tid.ToString();
            info.State = StepRunInfo.StepRunState.Started;
            FlowEvent infoEvent = new FlowEvent(
                FlowEvent.RUN_STARTED,
                stepRunDescriptor.WfId,
                stepRunDescriptor.StepId,
                info);
            _eventConsumer.PushEvent(infoEvent);

            _log.Trace("Set dryrun result for step '{0}'", stepRunDescriptor.StepId);
            StepRunResult result = new StepRunResult();
            result.Status = StepRunResult.ResultStatus.Unknown;
            result.ExternalId = tid.ToString();
            FlowEvent flowEvent = new FlowEvent(
                FlowEvent.RUN_FINISHED,
                stepRunDescriptor.WfId,
                stepRunDescriptor.StepId,
                result);

            _eventConsumer.PushEvent(flowEvent);
        }

    }
}