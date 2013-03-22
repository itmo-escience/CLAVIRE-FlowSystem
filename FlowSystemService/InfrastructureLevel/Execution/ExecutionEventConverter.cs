using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Easis.Common.DataContracts;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.FlowSystemService.ExecutionService;
using Easis.Wfs.FlowSystemService.PES;
using Easis.Wfs.FlowSystemService.ResourceBaseService;
using Easis.Wfs.FlowSystemService.StorageService;
using Easis.Wfs.FlowSystemService.Utils;
using Easis.Wfs.Interpreting;
using Eventing;
using MongoDB.Bson;
using NLog;
using Easis.Common;

namespace Easis.Wfs.FlowSystemService
{
    class ExecutionEventConverter : EventConverterBase
    {
        static readonly Logger _slog = LogManager.GetCurrentClassLogger();
        protected override bool InternalCanConvert(EventReport eventReport)
        {
            return String.Equals(eventReport.Source, "Execution", StringComparison.CurrentCultureIgnoreCase) &&
                String.Equals(eventReport.Topic, "WFStateUpdatedEvent", StringComparison.CurrentCultureIgnoreCase);
        }

        private Task ExecutionGetTask(ulong sequenceId, WfLog log)
        {
            // 2. Спросить у песа детали
            ExecutionService.ExecutionBrokerServiceClient cli = new ExecutionBrokerServiceClient();

            bool done = false;
            int attempts = 0;
            const int maxAttempts = 5;
            const int sleepAttempt = 2000;

            Task task = null;

            while (!done) // пытаемся maxAttempts раз получить информацию о цепочке
            {
                try
                {
                    Thread.Sleep(sleepAttempt);
                    task = cli.GetInfo(sequenceId);
                    done = true;
                }
                catch (Exception e)
                {
                    attempts++;
                    if (attempts > maxAttempts)
                    {
                        log.Error("Cannot get information about sequence {0} from PES. Maxattempts reached. Exception thrown.", sequenceId);
                        throw new Exception("Max number of attempts reached");
                    }

                    log.WarnException(String.Format("Cannot get information about sequence {0} from PES. Ignoring. Attempt {1}.", sequenceId, attempts), e);

                    attempts++;
                    Thread.Sleep(sleepAttempt);
                }
            }

            log.Trace("SequenceGetInfo returns {0}", task == null ? "NULL" : "valid object");

            if (task != null)
                log.Trace(task.ToJsonString());

            return task;

        }

        private StepRunInfo GetInfo(ulong sequenceId, WfLog log)
        {
            StepRunInfo ret;
            // typed copy
            LongRunningStepRunInfo lrret = null;

            StepRunDescriptor stepRunDescriptor = IdAccordanceDict.Instance.PopDescriptor(sequenceId);

            if (!stepRunDescriptor.IsLongRunning)
            {
                ret = new StepRunInfo();
            }
            else
            {
                lrret = new LongRunningStepRunInfo();
                ret = lrret;
            }

            ret.ExternalId = sequenceId.ToString();

            Task task = ExecutionGetTask(sequenceId, log);

            // PES вернул NULL
            if (task == null)
            {
                ret.State = StepRunInfo.StepRunState.Initialization;
                return ret;
            }

            if (task.AssignedResource != null)
            {
                ret.State = StepRunInfo.StepRunState.Started;

                ret.ResourceInfo = new ResourceRunInfo();
                ret.ResourceInfo.ResourceName = task.AssignedResource.ResourceName;
                ret.ResourceInfo.ResourceType = task.AssignedResource.ProviderName;

                ret.ResourceInfo.GeoCoordinates = task.AssignedResource.Location;

                if (task.AssignedNodes != null)
                    ret.ResourceInfo.CoresCount = task.AssignedNodes.Sum((nodeTotals => nodeTotals.CoresUsed));
                ret.ResourceInfo.NodesTotal = task.AssignedResource.NodesTotal;
                ret.ResourceInfo.ResourceDescription = task.AssignedResource.ResourceDescription;
                ret.ResourceInfo.SupportedArchitectures = task.AssignedResource.SupportedArchitectures;

                if (task.Time != null)
                {
                    if (task.Time.WhenStarted != null)
                        if (task.Time.WhenStarted.ContainsKey(TaskTimeMetric.Calculation))
                            ret.Started = task.Time.WhenStarted[TaskTimeMetric.Calculation];
                    if (task.Time.WhenFinished != null)
                        if (task.Time.WhenFinished.ContainsKey(TaskTimeMetric.Calculation))
                            ret.Ended = task.Time.WhenFinished[TaskTimeMetric.Calculation];
                }

                if(task.CurrentSchedule != null && task.CurrentSchedule.Estimation != null)
                {
                    if(task.CurrentSchedule.Estimation.ByModel != null && task.CurrentSchedule.Estimation.ByModel.CalculationTime != null)
                    {
                        ret.Estimation = task.CurrentSchedule.Estimation.ByModel.CalculationTime.Value;
                        ret.EstimationDispersion = task.CurrentSchedule.Estimation.ByModel.CalculationTime.Dispersion;
                    }
                }
            }

            if (task.AssignedNodes != null)
            {
                var l = new List<NodeRunInfo>();
                foreach (var assignedNode in task.AssignedNodes)
                {
                    l.Add(new NodeRunInfo
                              {
                                  CoresUsed = assignedNode.CoresUsed,
                                  NodeName = assignedNode.NodeName,
                                  SupportedArchitectures = assignedNode.SupportedArchitectures
                              });

                }
                ret.NodeInfos = l;
            }

            // -------------------------------
            // Long-running information
            // -------------------------------
            if (stepRunDescriptor.IsLongRunning)
            {
                lrret.Id = IdAccordanceDict.Instance.GetWfAndStepId(sequenceId).Second;

                ResourceBaseService.ResourceBaseServiceClient rbcli = new ResourceBaseServiceClient();
                if (task.AssignedNodes != null)
                {
                    if (task.AssignedNodes.Length > 0)
                    {
                        if (task.AssignedNodes.Length > 1)
                            _slog.Warn("Assigned nodes contains more than one element for long running task (only one is supported).");

                        log.Trace("Fetching info from resource base about node '{0}.{1}'", ret.ResourceInfo.ResourceName, task.AssignedNodes[0].NodeName);
                        ResourceNode rn = rbcli.GetResourceNodeByName(ret.ResourceInfo.ResourceName, task.AssignedNodes[0].NodeName);

                        //TODO: del
                        log.Trace(rn.ToJsonString());

                        var packagesInfo = rn.Packages;
                        foreach (var packageOnNode in packagesInfo)
                        {
                            if (packageOnNode.Name.ToLower() == stepRunDescriptor.PackageName.ToLower())
                            {
                                // searching for LR params
                                if (packageOnNode.Params.ContainsKey("LR_COMMAND_ENDPOINT"))
                                {
                                    lrret.CommandEndpoint = packageOnNode.Params["LR_COMMAND_ENDPOINT"];
                                }
                                else
                                {
                                    log.Error("Command endpoint is not specified for package '{0}' in resource base", stepRunDescriptor.PackageName);
                                    throw new InterpretionException(String.Format("Command endpoint is not specified for package '{0}' in resource base", stepRunDescriptor.PackageName));
                                }

                                if (packageOnNode.Params.ContainsKey("LR_PUBLISHING_ENDPOINT"))
                                {
                                    lrret.PublishingEndpoint = packageOnNode.Params["LR_PUBLISHING_ENDPOINT"];
                                }
                                else
                                {
                                    log.Error("Publish endpoint is not specified for package '{0}' in resource base", stepRunDescriptor.PackageName);
                                    throw new InterpretionException(String.Format("Command endpoint is not specified for package '{0}' in resource base", stepRunDescriptor.PackageName));
                                }

                                break;
                            }
                        }

                        log.Trace("Fetched info about lr task cmd at '{0}' pub at '{1}'", lrret.CommandEndpoint, lrret.PublishingEndpoint);
                    }
                    else
                    {
                        log.Error("Assigned node fetched from Execution is null. Cannot fetch info about long running task.");
                        throw new InterpretionException(
                            "Assigned node fetched from Execution is null. Cannot fetch info about long running task.");
                    }
                }
            }

            return ret;
        }

        private StepRunResult GetResult(ulong sequenceId, WfLog log)
        {
            StepRunResult ret = new StepRunResult();

            ret.ExternalId = sequenceId.ToString();

            Task task = ExecutionGetTask(sequenceId, log);

            // PES вернул NULL
            if (task == null)
            {
                ret.Status = StepRunResult.ResultStatus.Failed;
                return ret;
            }

            // Проверяем результат);
            switch (task.State)
            {
                case TaskState.Completed:
                    ret.Status = StepRunResult.ResultStatus.Completed;

                    // output files
                    foreach (var outFile in task.OutputFiles)
                    {
                        log.Trace("Found output file name:'{0}' slot:'{1}' storageid:{2}", outFile.FileName, outFile.SlotName, outFile.StorageId, outFile);
                        FileDescriptor fd = new FileDescriptor(null, outFile.FileName, outFile.StorageId);
                        ret.OutputFiles.Add(fd);
                    }

                    // output params
                    try
                    {
                        ret.OutputParams = task.OutputParams;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Exception while fetching output params. Ignoring.", ex);
                    }

                    // registering data -------------------------------------------
                    // TODO: fix new file registry logic [AL] durty hack
                    IList<DataEntry> des = new List<DataEntry>();
                    foreach (var of in ret.OutputFiles)
                    {
                        // form new pretty name
                        Utils.Pair<Guid, long> ids0 = IdAccordanceDict.Instance.GetWfAndStepId(sequenceId);
                        Guid WfId = ids0.First;
                        long StepId = ids0.Second;
                        string name = of.FileIdentifier;

                        DataEntry de = new DataEntry();
                        // durty hack nulls in name
                        de.Name = StepId + "_" + name;
                        de.Locator = of.NStorageId;
                        des.Add(de);
                    }
                    if (des.Count > 0)
                    {
                        try
                        {
                            StorageService.StorageServiceClient scli = new StorageServiceClient();
                            string[] ids = scli.RegisterDataBatch(des.ToArray());

                            log.Trace("Storage service returned {0} ids for {1} data entries", ids.Length, ret.OutputFiles.Count);

                            for (int i = 0; i < ret.OutputFiles.Count; i++)
                            {
                                ret.OutputFiles.ToArray()[i].StorageId = ids[i];
                            }
                            log.Trace("Registered new files in storage");
                        }
                        catch (Exception ex)
                        {
                            log.ErrorException("Couldn't register file in storage", ex);
                        }
                    }
                    else
                    {
                        log.Trace("No need to register files");
                    }

                    break;
                case TaskState.Defined:
                case TaskState.Scheduled:
                case TaskState.Started:
                case TaskState.Aborted:
                case TaskState.Failed:
                    ret.Status = StepRunResult.ResultStatus.Failed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ret;
        }

        protected override FlowEvent Convert(EventReport eventReport)
        {
            FlowEvent ret = null;

            EventReportSerializer eventReportSerializer = new EventReportSerializer();
            WFStateUpdatedEvent wfStateUpdatedEvent = (WFStateUpdatedEvent)eventReportSerializer.DeserializeObject(eventReport.Body, typeof(WFStateUpdatedEvent));

            // Выбираем из события песовский ID
            ulong sequenceId = ulong.Parse(wfStateUpdatedEvent.WFStepCode);

            _slog.Trace("Found event from Execution for StepId {0}. Trying to find accordance in id dict..", sequenceId);
            try
            {
                Utils.Pair<Guid, long> ids = IdAccordanceDict.Instance.GetWfAndStepId(sequenceId);

                Guid WfId = ids.First;
                long StepId = ids.Second;

                WfLog log = new WfLog(LogManager.GetCurrentClassLogger(), WfId);

                switch (wfStateUpdatedEvent.WFStateUpdatedType)
                {
                    case WFStateUpdatedTypeEnum.WFStepStarted:
                        try
                        {
                            // find out where it runs
                            StepRunInfo sri = GetInfo(sequenceId, log);
                            log.Trace("RunInfo has been successfully fetched");
                            ret = new FlowEvent(FlowEvent.RUN_STARTED, WfId, StepId, sri);
                        }
                        catch (InterpretionException ex)
                        {
                            ret = new FlowEvent(FlowEvent.ERROR, WfId, StepId, ex.Message);
                        }
                        break;
                    case WFStateUpdatedTypeEnum.WFStepFinished:
                    case WFStateUpdatedTypeEnum.WFStepError:
                        try
                        {
                            StepRunResult result = GetResult(sequenceId, log);
                            log.Trace("StepResult has been successfully fetched");
                            result.ErrorComment = wfStateUpdatedEvent.Comment;
                            ret = new FlowEvent(FlowEvent.RUN_FINISHED, WfId, StepId, result);
                        }
                        catch (InterpretionException ex)
                        {
                            ret = new FlowEvent(FlowEvent.ERROR, WfId, StepId, ex.Message);
                        }
                        break;
                    case WFStateUpdatedTypeEnum.WFStarted:
                    case WFStateUpdatedTypeEnum.WFFinished:
                        // ignore self events
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (KeyNotFoundException ex)
            {
                _slog.Error("Can't convert PES event. May be it is not for me. Rethrown.");
                throw ex;
            }

            _slog.Trace("Event {0} converted with {1} to {2}", eventReport, this.GetType(), ret);

            return ret;
        }
    }
}