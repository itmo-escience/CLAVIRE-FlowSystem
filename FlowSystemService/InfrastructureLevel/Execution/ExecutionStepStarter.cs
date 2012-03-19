using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using Easis.Common;
using Easis.Wfs.FlowSystemService.ExecutionService;
using Easis.Wfs.FlowSystemService.PES;
using Easis.Wfs.Interpreting;
using Ipse.Gui.Infra.Pes.Xsd;
using NLog;

namespace Easis.Wfs.FlowSystemService
{
    public class ExecutionStepStarter : IStepStarter
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        protected object _syncRoot = new object();

        public TaskDescription FormTaskDescription(StepRunDescriptor stepRunDescriptor)
        {
            TaskDescription td = new TaskDescription
                {
                    Method = stepRunDescriptor.MethodName,
                    Package = stepRunDescriptor.PackageName
                };

            #region ExecutionContext
            if (stepRunDescriptor.ExecutionContext != null)
            {
                td.UserId = stepRunDescriptor.ExecutionContext.UserId;
                
                td.UserCert = stepRunDescriptor.ExecutionContext.TempUserCert;

                if (stepRunDescriptor.ExecutionContext.Priority != null)
                {
                    // TODO: remove
                    _log.Trace("----------- {0}", stepRunDescriptor.ExecutionContext.Priority);
                    switch (stepRunDescriptor.ExecutionContext.Priority.ToLower())
                    {
                        case "urgent":
                            td.Priority = TaskPriority.Urgent;
                            break;
                        default:
                            td.Priority = TaskPriority.Normal;
                            break;
                    }
                }
                else
                    td.Priority = TaskPriority.Normal;

                td.ExecParams = new Dictionary<string, string>();
                foreach (var element in stepRunDescriptor.ExecutionContext.ExtraElements)
                {
                    td.ExecParams.Add(element.Key, element.Value);
                }
            }
            //toto: else
            #endregion


            td.LaunchMode = stepRunDescriptor.RunMode == StepRunMode.Meta ? TaskLaunchMode.Auto : TaskLaunchMode.Manual;

            //// NOTE: choosing PES working mode
            //if (stepRunDescriptor.RunParameters.Params.Count == 0)
            //{
            //    td.LaunchMode = TaskLaunchMode.Manual;
            //    _log.Trace("Manual launch mode was set (no params were defined)");
            //}
            //else
            //{
            //    td.LaunchMode = TaskLaunchMode.Auto;
            //    _log.Trace("Auto launch mode was set ({0} params were defined)", stepRunDescriptor.RunParameters.Params.Count);
            //}

            td.InputFiles = new TaskFileDescription[stepRunDescriptor.RunParameters.InputFiles.Count];
            int i = 0;
            // Заполняем входные слоты
            foreach (var inf in stepRunDescriptor.RunParameters.InputFiles)
            {
                td.InputFiles[i].SlotName = inf.RoleName;
                td.InputFiles[i].StorageId = inf.FileDescriptor.NStorageId;
                td.InputFiles[i].FileName = inf.FileDescriptor.FileIdentifier;

                _log.Trace("Packing input file info slot {0} sid {1} filename {2}", td.InputFiles[i].SlotName, td.InputFiles[i].StorageId);

                // TODO: dirty hack
                //td.InputFiles[i].FileName = String.Format("{0}.in", i);

                i++;
            }

            if (stepRunDescriptor.RunParameters.OutputFiles.Count == 0)
            {
                //TODO: sm is negodiai
                //td.OutputFiles = new TaskFileDescription[1];
                //td.OutputFiles[0].SlotName = "output";
                //td.OutputFiles[0].FileName = "output.out";
            }
            else
            {
                td.OutputFiles = new TaskFileDescription[stepRunDescriptor.RunParameters.OutputFiles.Count];
                i = 0;
                // Заполняем выходные слоты
                foreach (var inf in stepRunDescriptor.RunParameters.OutputFiles)
                {
                    td.OutputFiles[i].SlotName = inf.RoleName;
                    td.OutputFiles[i].StorageId = inf.FileDescriptor.NStorageId;
                    //NOTE: fix later
                    td.OutputFiles[i].FileName = inf.RoleName;

                    i++;
                }
            }

            td.Params = new Dictionary<string, string>();
            i = 0;
            // Заполняем параметры
            foreach (KeyValuePair<string, string> paramPair in stepRunDescriptor.RunParameters.Params)
            {
                string packageDomainTerm = paramPair.Key;
                string value = paramPair.Value;

                if (value != null) td.Params.Add(packageDomainTerm, value);
            }

            return td;
        }

        public virtual void StartStep(StepRunDescriptor stepRunDescriptor)
        {
            ExecutionService.ExecutionBrokerServiceClient ec = new ExecutionBrokerServiceClient();
            // 1. new id
            ulong tid = ec.GetNewTaskId();

            TaskDescription td = FormTaskDescription(stepRunDescriptor);

            td.TaskId = tid;
            td.WfId = stepRunDescriptor.WfId.ToString();

            _log.Trace("Defining step using Execution.");
            _log.Trace(td.ToJsonString());

            // 2. define
            ec.Define(new TaskDescription[] { td }, null);

            // Важно соответствие ставить до вызова запускатора
            // иначе обработчик события срабатывает раньше и необходимого ключа не находится
            lock (_syncRoot)
            {
                _log.Trace("Adding triplet ({0}, {1}, {2}) to Id dict", stepRunDescriptor.WfId, stepRunDescriptor.StepId, tid);
                IdAccordanceDict.Instance.AddIdTriplet(stepRunDescriptor.WfId, stepRunDescriptor.StepId, tid);

                IdAccordanceDict.Instance.SaveDescriptor(tid, stepRunDescriptor);
            }

            _log.Trace("Staring step using Execution.");
            // 3. execute
            ec.ExecuteAsync(new ulong[] { tid });
        }

        public virtual bool IsDry
        {
            get { return false; }
            set { throw new NotSupportedException(); }
        }

        public virtual void SetEventConsumer(IEventConsumer eventConsumer)
        {
            throw new NotSupportedException();
        }
    }
}
