using System;
using Easis.Common.DataContracts;


namespace Easis.Wfs.Interpreting
{
    public enum StepRunMode
    {
        Meta,
        Raw
    }

    /// <summary>
    /// Дескриптор запускаемого шага.
    /// Конвертируется в задание системы исполнения. (для PES - в xml)
    /// </summary>
    public class StepRunDescriptor
    {
        public ExecutionContext ExecutionContext { get; set; }

        public Guid WfId { get; private set; }
        public long StepId { get; private set; }

        public long[] DependsOn { get; set; }


        /// <summary>
        /// Is set after dry run
        /// </summary>
        public ulong? RealStepId { get; set; }

        public string PackageName { get; set; }
        public string MethodName { get; set; }

        public StepRunMode RunMode { get; set; }
        private StepRunParameters _runParameters;

        public bool IsLongRunning { get; set; }

        public StepRunParameters RunParameters { get { return _runParameters; } set { _runParameters = value; } }

        public StepRunDescriptor(Guid wfId, long stepId)
        {
            WfId = wfId;
            StepId = stepId;

            IsLongRunning = false;

            _runParameters = new StepRunParameters();
        }

        public override string ToString()
        {
            string log = "OutFiles: [ ";
            foreach (var outputFile in _runParameters.OutputFiles)
            {
                log += "( " + outputFile.RoleName + " " + outputFile.FileDescriptor.FileIdentifier + " " +
                       outputFile.FileDescriptor.Id + " ) ";
            }
            log += "]";
            return String.Format("WF#{0}.{1} run {2} {3}", WfId, StepId, PackageName,log);
        }
    }
}
