using System;
using System.Collections.Generic;
using Easis.Wfs.FlowSystemService.PES;
using Easis.Wfs.Interpreting;
using Ipse.Gui.Infra.Pes.Xsd;
using NLog;

namespace Easis.Wfs.FlowSystemService
{
    [Obsolete]
    public class PesStepStarter : IStepStarter
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private object _syncRoot = new object();

        public sequenceRunType FormSequenceRunRequestXML(StepRunDescriptor stepRunDescriptor)
        {
            sequenceRunType result = new sequenceRunType
            {
                //TODO: изменить - грязный хак
                id = 0,
                step = new List<sequenceRunTypeStep>(),
                timeLimit = 0,
                timeLimitSpecified = true,                
            };

            // Делаем один Step

            // создаем структуру Xml для запуска шага
            sequenceRunTypeStep step = new sequenceRunTypeStep
                                           {
                                               @in = new List<inOutType>(),
                                               @out = new List<inOutType>(),
                                               app = new withNameType {name = stepRunDescriptor.PackageName},
                                               method = new withNameType {name = stepRunDescriptor.MethodName},
                                               order = 0,                                               
                                               param = new List<sequenceRunTypeStepParam>(),
                                               type = stepType.exec,
                                               mode = stepRunDescriptor.RunMode == StepRunMode.Meta ? stepModeType.Auto : stepModeType.Manual,                                               
                                           };

            // Заполняем входные слоты
            foreach (var inf in stepRunDescriptor.RunParameters.InputFiles)
            {
                step.@in.Add(new inOutType
                {
                    internalId = (ulong) inf.FileDescriptor.Id,
                    slotName = inf.RoleName,
                    storageId = inf.FileDescriptor.NStorageId,
                    type = inf.RoleName
                });
            }

            // Заполняем выходные слоты
            foreach (var inf in stepRunDescriptor.RunParameters.OutputFiles)
            {
                step.@out.Add(new inOutType
                {
                    internalId = (ulong) Math.Abs(inf.FileDescriptor.Id.Value),
                    slotName = inf.RoleName,
                    storageId = null,
                    type = "", // TODO: проверить, почему тут пустая строка
                });
            }

            // Заполняем параметры
            foreach (KeyValuePair<string, string> paramPair in stepRunDescriptor.RunParameters.Params)
            {
                string packageDomainTerm = paramPair.Key;
                string value = paramPair.Value;

                if (value!=null)
                    step.param.Add(new sequenceRunTypeStepParam
                    {
                        name = packageDomainTerm,
                        value = value,
                    });
            }

            result.step.Add(step);

            return result;
        }


        public void StartStep(StepRunDescriptor stepRunDescriptor)
        {
            PES.SequenceService ss = new SequenceService();
            sequenceRunType sequenceRun = FormSequenceRunRequestXML(stepRunDescriptor);

            sequenceRun.id = (ulong) DateTime.Now.Ticks % 100000000 ;

            string text = sequenceRun.Serialize();

            _log.Trace("Staring step using PES. Command text = \n{0}",text);

            // Важно соответствие ставить до вызова запускатора
            // иначе обработчик события срабатывает раньше и необходимого ключа не находится
            lock (_syncRoot)
            {
                _log.Trace("Adding triplet ({0}, {1}, {2}) to Id dict", stepRunDescriptor.WfId, stepRunDescriptor.StepId, sequenceRun.id);
                IdAccordanceDict.Instance.AddIdAccordance(stepRunDescriptor.WfId, stepRunDescriptor.StepId, sequenceRun.id);
            }

            ss.SequenceRunAsync(text);

        }

        public bool IsDry
        {
            get { return false; }
            set { throw new NotImplementedException(); }
        }

        public void SetEventConsumer(IEventConsumer eventConsumer)
        {
            throw new NotImplementedException();
        }
    }
}
