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
    public class DryExecutionStepStarter : ExecutionStepStarter
    {
        //class EBSC : IExecutionBrokerService
        //{
        //    public void ExecuteAsync(ulong[] ids)
        //    {
                
        //    }

        //    public bool MagicHappens()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IAsyncResult BeginMagicHappens(AsyncCallback callback, object asyncState)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public bool EndMagicHappens(IAsyncResult result)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Define(TaskDescription[] tasks, TaskDependency[] dependencies)
        //    {
                
        //    }

        //    public IAsyncResult BeginDefine(TaskDescription[] tasks, TaskDependency[] dependencies, AsyncCallback callback, object asyncState)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void EndDefine(IAsyncResult result)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Execute(ulong[] taskIds)
        //    {
                
        //    }

        //    public IAsyncResult BeginExecute(ulong[] taskIds, AsyncCallback callback, object asyncState)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void EndExecute(IAsyncResult result)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Abort(ulong[] taskId)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IAsyncResult BeginAbort(ulong[] taskId, AsyncCallback callback, object asyncState)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void EndAbort(IAsyncResult result)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public Task GetInfo(ulong taskId)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IAsyncResult BeginGetInfo(ulong taskId, AsyncCallback callback, object asyncState)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public Task EndGetInfo(IAsyncResult result)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public BriefTaskInfo[] GetBriefTaskList()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IAsyncResult BeginGetBriefTaskList(AsyncCallback callback, object asyncState)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public BriefTaskInfo[] EndGetBriefTaskList(IAsyncResult result)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    private static ulong cur = 1000;

        //    public ulong GetNewTaskId()
        //    {
        //        cur++;
        //        return cur;
        //    }

        //    public IAsyncResult BeginGetNewTaskId(AsyncCallback callback, object asyncState)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public ulong EndGetNewTaskId(IAsyncResult result)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void Update()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public IAsyncResult BeginUpdate(AsyncCallback callback, object asyncState)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public void EndUpdate(IAsyncResult result)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}


        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private IEventConsumer _eventConsumer;

        public override bool IsDry { get; set; }

        public DryExecutionStepStarter(IEventConsumer eventConsumer)
        {
            _eventConsumer = eventConsumer;
        }

        public override void SetEventConsumer(IEventConsumer eventConsumer)
        {
            _eventConsumer = eventConsumer;
        }


        public override void StartStep(StepRunDescriptor stepRunDescriptor)
        {
            //EBSC ec = new EBSC();// new ExecutionBrokerServiceClient();
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
                    tid = IdAccordanceDict.Instance.GetRealId(stepRunDescriptor.WfId, stepRunDescriptor.StepId);
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

                if(!IsDry)
                    IdAccordanceDict.Instance.SaveDescriptor(tid, stepRunDescriptor);
            }
            
            
            if (!IsDry)
            {
                _log.Trace("Staring step using Execution.");
                // 3. execute
                ec.ExecuteAsync(new ulong[] {tid});
            }else
            {
                _log.Trace("Set dryrun info for step '{0}'", stepRunDescriptor.StepId);
                
                StepRunInfo info;
                
                if(stepRunDescriptor.IsLongRunning)
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
