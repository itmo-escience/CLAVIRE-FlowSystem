using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.FlowSystemService;
using Easis.Wfs.FlowSystemService.Utils;
using Eventing;
using Easis.Wfs.Interpreting;
using Easis.Wfs.FlowSystemService.PES;
using NLog;

namespace Easis.Wfs.FlowSystemService
{
    [Obsolete]
    class PESEventConverter : EventConverterBase
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();
        protected override bool InternalCanConvert(EventReport eventReport)
        {
            return String.Equals(eventReport.Source, "PES", StringComparison.CurrentCultureIgnoreCase);
        }

        private StepRunResult GetResult(ulong sequenceId)
        {
            StepRunResult ret = new StepRunResult();

            ret.ExternalId = sequenceId.ToString();

            // 2. Спросить у песа детали
            SequenceService sequenceService = new SequenceService();

            bool done = false;
            int attempts = 0;
            const int maxAttempts = 5;
            const int sleepAttempt = 2000;
            SequenceGetInfoResult sequenceRunInfo = null;

            while (!done) // пытаемся maxAttempts раз получить информацию о цепочке
            {
                try
                {
                    Thread.Sleep(sleepAttempt);
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

            _log.Trace("SequenceGetInfo returns {0}", sequenceRunInfo == null ? "NULL" : "valid object");

            // PES вернул NULL
            if (sequenceRunInfo == null)
            {
                ret.Status = StepRunResult.ResultStatus.Failed;
                return ret;
            }

            var CorrectedResult = sequenceRunInfo.result;
            // Checking correctness of set result status
            // Only euristics. 
            switch (sequenceRunInfo.result)
            {
                case stepResult.completed:
                    ret.Status = StepRunResult.ResultStatus.Completed;

                    // Euristics. See ticket:http://194.85.163.238/nano/ticket/41
                    long SummFileSize = 0;
                    string SummFileName = "";

                    foreach (var outFile in sequenceRunInfo.step[0].@out)
                    {
                        SummFileSize += outFile.fileSize;
                        SummFileName += outFile.fileName;
                    }
                    if (SummFileName == "" && SummFileSize == 0)
                    {
                        _log.Warn("PES completed 'result' was reinterpreted to 'failed'");
                        CorrectedResult = stepResult.failed;
                    }
                    break;
            }


            // Проверяем результат
            switch (CorrectedResult)
            {
                case stepResult.completed:
                    ret.Status = StepRunResult.ResultStatus.Completed;

                    foreach (var outFile in sequenceRunInfo.step[0].@out)
                    {
                        _log.Trace("Found output file ({3}) in result: {0} {1} {2}", outFile.slotName, outFile.storageId, outFile.internalId,sequenceRunInfo.step[0].@out.Length);
                        FileDescriptor fd = new FileDescriptor(null, outFile.fileName, outFile.storageId);
                        ret.OutputFiles.Add(fd);
                    }

                    break;
                case stepResult.failed:
                case stepResult.notStarted:
                case stepResult.started:
                case stepResult.aborted:
                case stepResult.timeLimitExceeded:
                    ret.Status = StepRunResult.ResultStatus.Failed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //if (new[] { stepResult.completed }.Contains(sequenceRunInfo.result))
            //    ret.Status = StepRunResult.ResultStatus.Completed;
            //else
            //{
            //    ret.Status = StepRunResult.ResultStatus.Failed;
            //}


            return ret;
        }

        protected override FlowEvent Convert(EventReport eventReport)
        {
            FlowEvent ret = null;

            EventReportSerializer eventReportSerializer = new EventReportSerializer();
            WFStateUpdatedEvent wfStateUpdatedEvent = (WFStateUpdatedEvent)eventReportSerializer.DeserializeObject(eventReport.Body, typeof(WFStateUpdatedEvent));

            // Выбираем из события песовский ID
            //TODO: Убрать этот ужас
            // sid#1810
            ulong sequenceId = ulong.Parse(wfStateUpdatedEvent.WFStepCode.Substring(4));

            _log.Trace("Found event from PES for StepId {0}. Trying to find accordance in id dict..", sequenceId);
            try
            {
                Pair<Guid, long> ids = IdAccordanceDict.Instance.GetWfAndStepId(sequenceId);

                Guid WfId = ids.First;
                long StepId = ids.Second;

                switch (wfStateUpdatedEvent.WFStateUpdatedType)
                {
                    case WFStateUpdatedTypeEnum.WFStepStarted:
                        ret = new FlowEvent(FlowEvent.RUN_STARTED, WfId, StepId);
                        break;
                    case WFStateUpdatedTypeEnum.WFStepFinished:
                    case WFStateUpdatedTypeEnum.WFStepError:
                        StepRunResult result = GetResult(sequenceId);
                        ret = new FlowEvent(FlowEvent.RUN_FINISHED, WfId, StepId, result);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (KeyNotFoundException ex)
            {
                _log.Error("Can't convert PES event. May be it is not for me. Rethrown.");
                throw ex;
            }

            _log.Trace("Event {0} converted with {1} to {2}", eventReport, this.GetType(), ret);

            return ret;
        }
    }
}
