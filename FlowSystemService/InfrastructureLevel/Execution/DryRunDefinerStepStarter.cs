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
    public class ObsoleteDryRunDefinerStepStarter : ExecutionStepStarter
    {

        private IEventConsumer _eventConsumer;

        public ObsoleteDryRunDefinerStepStarter(IEventConsumer eventConsumer)
        {
            _eventConsumer = eventConsumer;
        }

        public override void SetEventConsumer(IEventConsumer eventConsumer)
        {
            _eventConsumer = eventConsumer;
        }

        public override bool IsDry
        {
            get { return true; }
        }

        public override void StartStep(StepRunDescriptor stepRunDescriptor)
        {
            WfLog _log = new WfLog(LogManager.GetCurrentClassLogger(), stepRunDescriptor.WfId);

            ExecutionBrokerServiceClient ec = new ExecutionBrokerServiceClient();

            bool isIdNew = true;
            // 1. new id
            ulong tid;
            if (IsDry)
                tid = ec.GetNewTaskId();
            else
            {
                try
                {
                    tid = IdAccordanceDict.Instance.GetRealTaskId(stepRunDescriptor.WfId, stepRunDescriptor.StepId);
                    isIdNew = false;
                }
                catch (Exception ex)
                {
                    tid = ec.GetNewTaskId();
                }
            }

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
                if (isIdNew)
                {
                    _log.Trace("Adding triplet ({0}, {1}, {2}) to Id dict", stepRunDescriptor.WfId,
                               stepRunDescriptor.StepId, tid);
                    IdAccordanceDict.Instance.AddIdTriplet(stepRunDescriptor.WfId, stepRunDescriptor.StepId, tid);
                }

                if (!IsDry)
                    IdAccordanceDict.Instance.SaveDescriptor(tid, stepRunDescriptor);
            }


            if (!IsDry)
            {
                _log.Trace("Staring step using Execution.");
                // 3. execute
                ec.ExecuteAsync(new ulong[] { tid });
            }
            else
            {
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
}
