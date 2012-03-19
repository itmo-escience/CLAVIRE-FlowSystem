using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Easis.Common.DataContracts;
using Easis.Wfs.Interpreting;
using Eventing;
using NLog;

namespace Easis.Wfs.FlowSystemService
{
    /// <summary>
    /// hosting:
    /// Сервис обладает состоянием, поэтому должен работать беспрерывно.
    /// </summary>
    public class FlowSystemService : IFlowSystemService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private ExtEventListener _eventListener = new ExtEventListener(JobExecutor.Instance);

        public Guid PushJob(JobRequest jobRequest)
        {
            try
            {
                return JobExecutor.Instance.PushJob(jobRequest);
            }
            catch (Exception e)
            {
                Log.ErrorException("PushJob error", e);
                throw e;
            }
        }

        public JobDescription GetJobDescription(Guid wfId)
        {
            try
            {
                return JobExecutor.Instance.GetJobDescription(wfId);
            }
            catch (Exception e)
            {
                Log.ErrorException("GetJobDescription error", e);
                throw e;
            }

        }

        public JobDescription[] GetJobDescriptions()
        {
            try
            {
                return JobExecutor.Instance.GetJobDescriptions();
            }
            catch (Exception e)
            {
                Log.ErrorException("GetJobDescriptions error", e);
                throw e;
            }
        }

        public LongRunningTaskInfo[] GetLongRunningTasks(string packageName)
        {
            List<LongRunningTaskInfo> ret = new List<LongRunningTaskInfo>();
            try
            {
                var ris = JobExecutor.Instance.GetLongRunningRunInfos();
                var descriptions = JobExecutor.Instance.GetJobDescriptions();

                foreach (var ri in ris)
                {
                    foreach (var longRunningStepRunInfo in ri.Value)
                    {
                        LongRunningTaskInfo ti = new LongRunningTaskInfo();
                        ti.PublishingEndpoint = longRunningStepRunInfo.PublishingEndpoint;
                        ti.CommandEndpoint = longRunningStepRunInfo.CommandEndpoint;

                        JobDescription jd = descriptions.Where(description => description.WfId == ri.Key).First();
                        LongRunningStepRunInfo info = longRunningStepRunInfo;
                        NodeDescription nd = jd.Nodes.Where(nodeDescription => nodeDescription.StepId == info.Id).First();
                        ti.Description = nd;
                        ti.WfId = ri.Key;
                        ret.Add(ti);
                    }

                }
            }
            catch (Exception e)
            {
                Log.ErrorException("GetJobDescriptions error", e);
                throw e;
            }
            return ret.ToArray();
        }

        public void Control(string action, Guid WfId, long blockId)
        {
            try
            {
                switch (action.ToLower())
                {
                    case "pause":
                        JobExecutor.Instance.Pause(WfId, blockId);
                        break;
                    case "stop":
                        JobExecutor.Instance.Stop(WfId, blockId);
                        break;
                    case "resume":
                        JobExecutor.Instance.Resume(WfId, blockId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unsupported action {0}", action);
                }
            }
            catch (Exception e)
            {
                Log.ErrorException("Control error", e);
                throw e;
            }

        }

        public void Notify(Notification notification)
        {
            try
            {
                _eventListener.Notify(notification);
            }
            catch (Exception e)
            {
                Log.ErrorException("Notify error", e);
                throw e;
            }

        }
    }
}
