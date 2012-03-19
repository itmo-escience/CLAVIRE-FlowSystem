using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Eventing;
using Easis.Wfs.Interpreting;
using ExternalEventListener.PES;

namespace ExternalEventListener
{
    class PESEventConverter : EventConverterBase
    {
        protected override bool CanConvert(EventReport eventReport)
        {
            return String.Equals(eventReport.Source, "PES", StringComparison.CurrentCultureIgnoreCase);
        }

        protected override FlowEvent Convert(EventReport eventReport)
        {
            // 1. Проверить статус

            EventReportSerializer eventReportSerializer = new EventReportSerializer();
            WFStateUpdatedEvent wfStateUpdatedEvent = (WFStateUpdatedEvent)eventReportSerializer.DeserializeObject(eventReport.Body, typeof(WFStateUpdatedEvent));

            // Выбираем из события песовский ID
            //TODO: Убрать этот ужас
            // sid#1810
            ulong sequenceId = ulong.Parse(wfStateUpdatedEvent.WFStepCode.Substring(4));

            // 2. Спросить у песа детали
            PES.SequenceServiceSoapClient sequenceService = new SequenceServiceSoapClient();

            bool done = false;
            int attempts = 0;
            const int maxAttempts = 5;
            const int sleepAttempt = 2000;
            SequenceGetInfoResult sequenceRunInfo = null;
            while (!done) // пытаемся maxAttempts раз получить информацию о цепочке
            {
                try
                {
                    sequenceRunInfo = sequenceService.SequenceGetInfo(sequenceId);
                    done = true;
                }
                catch (Exception e)
                {
                    attempts++;
                    if (attempts > maxAttempts)
                    {
                        _log.Error("Cannot get information about sequence {0} from PES. Maxattempts reached. Exception thrown.", sequenceId);
                        throw new Exception("Max number of attempts reached");
                    }

                    _log.Warn("Cannot get information about sequence {0} from PES. Ignoring. Attempt {1}.", sequenceId, attempts);

                    attempts++;
                    Thread.Sleep(sleepAttempt);
                }
            }

            // считаем, что получили результат -> формируем StepRunResult
            StepRunResult stepRunResult = new StepRunResult();
            
            // Проверяем результат
            switch(sequenceRunInfo.result)
            {
                case stepResult.completed:
                    stepRunResult.Status = StepRunResult.ResultStatus.Completed;
                    break;
                case stepResult.failed:
                case stepResult.notStarted:
                case stepResult.started:
                case stepResult.aborted:
                case stepResult.timeLimitExceeded:
                    stepRunResult.Status = StepRunResult.ResultStatus.Failed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            stepRunResult.StatusComment = sequenceRunInfo.result.ToString();


            // 3. Установить соответствие ID

            //TODO: realize)
            throw new NotImplementedException();
        }
    }
}
