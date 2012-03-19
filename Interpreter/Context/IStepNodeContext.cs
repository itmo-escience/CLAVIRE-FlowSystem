using System;
using Easis.Common.DataContracts;

namespace Easis.Wfs.Interpreting
{
    public interface IStepNodeContext : INodeContext
    {
        IPackageRegistry PackageRegistry { get; }
        IStepStarter StepStarter { get; }
        IFileRegistry FileRegistry { get; }
        Guid WfId { get; }
        ExecutionContext ExecutionContext { get; set; }
    }

    public class StepNodeContext : IStepNodeContext
    {
        public IInternalEventGenerator InternalEventGenerator { get; set; }
        public ICodeInterpreter CodeInterpreter { get; set; }
        public ExecutionContext ExecutionContext { get; set; }

        public INodeGraphController NodeGraphController { get; set; }

        public IPackageRegistry PackageRegistry { get; set; }
        public IStepStarter StepStarter { get; set; }
        public IFileRegistry FileRegistry { get; set; }
        public Guid WfId { get; set; }
        
        public object Clone()
        {
            StepNodeContext snc = new StepNodeContext();
            snc.InternalEventGenerator = InternalEventGenerator;
            snc.CodeInterpreter = CodeInterpreter;
            snc.ExecutionContext = ExecutionContext;
            snc.NodeGraphController = NodeGraphController;
            snc.PackageRegistry = PackageRegistry;
            snc.StepStarter = StepStarter;
            snc.FileRegistry = FileRegistry;
            snc.WfId = WfId;
            return snc;
        }
    }
}