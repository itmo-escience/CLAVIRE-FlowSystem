using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.Interpreting
{
    public class GeneralContext : INodeContext, IStepNodeContext, ISinkNodeContext
    {
        public IInternalEventGenerator InternalEventGenerator { get; set; }
        public ICodeInterpreter CodeInterpreter { get; set; }
        public IPackageRegistry PackageRegistry { get; set; }
        public IStepStarter StepStarter { get; set; }
        public IFileRegistry FileRegistry { get; set; }
        public long WfId { get; set; }
        public DeclarativeInterpreter DeclarativeInterpreter { get; set; }
        public FlowSystemContext FlowSystemContext { get; set; }
    }
}
