using System;

namespace Easis.Wfs.Interpreting
{
    public interface ISinkNodeContext : INodeContext
    {
        DeclarativeInterpreter DeclarativeInterpreter { get; }
    }

    public class SinkNodeContext : ISinkNodeContext
    {
        public IInternalEventGenerator InternalEventGenerator { get; set; }
        public ICodeInterpreter CodeInterpreter { get; set; }
        public INodeGraphController NodeGraphController { get; set; }
        public DeclarativeInterpreter DeclarativeInterpreter { get; set; }
        
        public object Clone()
        {
            SinkNodeContext nc = new SinkNodeContext();
            nc.CodeInterpreter = CodeInterpreter;
            nc.InternalEventGenerator = InternalEventGenerator;
            nc.NodeGraphController = NodeGraphController;
            nc.DeclarativeInterpreter = DeclarativeInterpreter;
            return nc;
        }
    }
}