using System;

namespace Easis.Wfs.Interpreting
{
    public interface IGlobalContext
    {
        IPackageRegistry PackageRegistry { get; set; }
        IStepStarter StepStarter { get; set; }
        IFileRegistry FileRegistry { get; set; }
        FlowSystemContext FlowSystemContext { get; set; }
        ILongRunningController LongRunningController { get; set; }
        IStorage Storage { get; set; }
        TimeSpan ThreadWaitForEventTimeout { get; set; }

        // TODO: make createfrom instead of create
        IStepNodeContext CreateStepNodeContext(Guid wfId, ICodeInterpreter codeInterpreter,
                                               IInternalEventGenerator internalEventGenerator, INodeGraphController nodeGraphController);

        ILongRunningStepNodeContext CreateLongRunningStepNodeContext(Guid wfId, ICodeInterpreter codeInterpreter,
                                                                     IInternalEventGenerator internalEventGenerator,
                                                                     INodeGraphController nodeGraphController);

    }

    public class GlobalContext : IGlobalContext
    {
        public IPackageRegistry PackageRegistry { get; set; }
        public IStepStarter StepStarter { get; set; }
        public IFileRegistry FileRegistry { get; set; }
        public FlowSystemContext FlowSystemContext  { get; set; }
        public ILongRunningController LongRunningController { get; set; }
        public IStorage Storage { get; set; }
        public TimeSpan ThreadWaitForEventTimeout { get; set; }

        public IStepNodeContext CreateStepNodeContext(Guid wfId, ICodeInterpreter codeInterpreter, IInternalEventGenerator internalEventGenerator, INodeGraphController nodeGraphController)
        {
            StepNodeContext ret = new StepNodeContext
                                      {
                                          CodeInterpreter = codeInterpreter,
                                          WfId = wfId,
                                          StepStarter = StepStarter,
                                          PackageRegistry = PackageRegistry,
                                          FileRegistry = FileRegistry,
                                          InternalEventGenerator = internalEventGenerator,
                                          NodeGraphController = nodeGraphController
                                      };
            return ret;
        }

        public ILongRunningStepNodeContext CreateLongRunningStepNodeContext(Guid wfId, ICodeInterpreter codeInterpreter, IInternalEventGenerator internalEventGenerator, INodeGraphController nodeGraphController)
        {
            LongRunningStepNodeContext ret = new LongRunningStepNodeContext
            {
                CodeInterpreter = codeInterpreter,
                WfId = wfId,
                StepStarter = StepStarter,
                PackageRegistry = PackageRegistry,
                FileRegistry = FileRegistry,
                InternalEventGenerator = internalEventGenerator,
                NodeGraphController = nodeGraphController,
                LongRunningController = LongRunningController,
                Storage = Storage
            };
            return ret;
        }
    }
}
