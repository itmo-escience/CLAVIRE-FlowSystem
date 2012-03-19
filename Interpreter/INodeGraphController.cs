using Easis.Wfs.Interpreting.Nodes;

namespace Easis.Wfs.Interpreting
{
    public interface INodeGraphController
    {
        void AddNodeDeffered(NodeBase nodeBase);
        StepNode GetStepNodeByName(string name);
        NodeBase GetNodeById(long id);
        LongRunningStepNode[] GetLongRunningNodes();
    }
}