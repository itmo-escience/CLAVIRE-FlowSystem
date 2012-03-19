using System;
using Easis.Common.DataContracts;

namespace Easis.Wfs.Interpreting
{
    public interface ILongRunningStepNodeContext : IStepNodeContext
    {
        ILongRunningController LongRunningController { get; }
        IStorage Storage { get; set; }
    }

    public class LongRunningStepNodeContext : ILongRunningStepNodeContext
    {
        public IInternalEventGenerator InternalEventGenerator { get; set; }
        public ICodeInterpreter CodeInterpreter { get; set; }
        public ExecutionContext ExecutionContext { get; set; }

        public INodeGraphController NodeGraphController { get; set; }

        public IPackageRegistry PackageRegistry { get; set; }
        public IStepStarter StepStarter { get; set; }
        public IFileRegistry FileRegistry { get; set; }
        public Guid WfId { get; set; }

        public ILongRunningController LongRunningController { get; set; }
        public IStorage Storage { get; set; }
        
        public object Clone()
        {
            LongRunningStepNodeContext snc = new LongRunningStepNodeContext();
            snc.InternalEventGenerator = InternalEventGenerator;
            snc.CodeInterpreter = CodeInterpreter;
            snc.ExecutionContext = ExecutionContext;
            snc.NodeGraphController = NodeGraphController;
            snc.PackageRegistry = PackageRegistry;
            snc.StepStarter = StepStarter;
            snc.FileRegistry = FileRegistry;
            snc.WfId = WfId;

            snc.LongRunningController = LongRunningController;
            snc.Storage = Storage;
            return snc;
        }
    }
}