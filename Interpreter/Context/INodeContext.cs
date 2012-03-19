using System;

namespace Easis.Wfs.Interpreting
{
    public interface INodeContext : ICloneable
    {
        IInternalEventGenerator InternalEventGenerator { get; }
        ICodeInterpreter CodeInterpreter { get; }
        INodeGraphController NodeGraphController { get; }
    }

    public class NodeContext : INodeContext
    {
        public IInternalEventGenerator InternalEventGenerator { get; set; }
        public ICodeInterpreter CodeInterpreter { get; set; }
        public INodeGraphController NodeGraphController { get; set; }

        public virtual object Clone()
        {
            NodeContext nc = new NodeContext();
            nc.CodeInterpreter = CodeInterpreter;
            nc.InternalEventGenerator = InternalEventGenerator;
            nc.NodeGraphController = NodeGraphController;
            return nc;
        }
    }
}
