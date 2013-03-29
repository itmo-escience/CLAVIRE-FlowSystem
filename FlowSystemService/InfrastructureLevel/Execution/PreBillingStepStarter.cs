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
    public class PreBillingStepStarter : ExecutionStepStarter
    {
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
            tid = ec.GetNewTaskId();

            TaskDescription td = FormTaskDescription(stepRunDescriptor);
            td.TaskId = tid;
            td.WfId = stepRunDescriptor.WfId.ToString();

            // Важно соответствие ставить до вызова запускатора
            // иначе обработчик события срабатывает раньше и необходимого ключа не находится
            lock (_syncRoot)
            {
                _log.Trace("Adding triplet ({0}, {1}, {2}) to Id dict", stepRunDescriptor.WfId, stepRunDescriptor.StepId, tid);
                IdAccordanceDict.Instance.AddIdAccordance(stepRunDescriptor.WfId, stepRunDescriptor.StepId, tid, JobRunMode.PreBilling);

                IdAccordanceDict.Instance.SaveDescriptor(tid, stepRunDescriptor);
            }

            // 2. define
            _log.Trace("Defining step {0} for prebilling using Execution.", tid);
            _log.Trace(td.ToJsonString());
            ec.DefineTask(td);
            _log.Trace("Step {0} has been defined, waiting for prebilling scheduling event..", tid);
        }
    }
}