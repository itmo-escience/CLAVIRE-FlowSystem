using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common;
using Easis.Wfs.FlowSystemService.ExecutionService;
using Easis.Wfs.Interpreting;
using NLog;

namespace Easis.Wfs.FlowSystemService
{
    public class NormalExecutionStepStarter : ExecutionStepStarter
    {
        public override bool IsDry
        {
            get { return false; }
        }

        public override void StartStep(StepRunDescriptor stepRunDescriptor)
        {
            WfLog _log = new WfLog(LogManager.GetCurrentClassLogger(), stepRunDescriptor.WfId);

            ExecutionBrokerServiceClient ec = new ExecutionBrokerServiceClient();

            bool isIdNew = true;
            // 1. new id
            ulong tid;
            try
            {
                tid = IdAccordanceDict.Instance.GetRealTaskId(stepRunDescriptor.WfId, stepRunDescriptor.StepId);
                isIdNew = false;
            }
            catch (Exception ex)
            {
                tid = ec.GetNewTaskId();
            }

            TaskDescription td = FormTaskDescription(stepRunDescriptor);
            td.TaskId = tid;
            td.WfId = stepRunDescriptor.WfId.ToString();

            _log.Trace("Defining step using Execution.");
            _log.Trace(td.ToJsonString());

            // 2. define
            ec.DefineTask(td);

            // Важно соответствие ставить до вызова запускатора
            // иначе обработчик события срабатывает раньше и необходимого ключа не находится
            lock (_syncRoot)
            {
                if (isIdNew)
                {
                    _log.Trace("Adding triplet ({0}, {1}, {2}) to Id dict", stepRunDescriptor.WfId, stepRunDescriptor.StepId, tid);
                    IdAccordanceDict.Instance.AddIdTriplet(stepRunDescriptor.WfId, stepRunDescriptor.StepId, tid);
                }

                IdAccordanceDict.Instance.SaveDescriptor(tid, stepRunDescriptor);
            }

            _log.Trace("Staring step using Execution.");
            // 3. execute
            ec.ExecuteAsync(new ulong[] { tid });
        }
    }
}
