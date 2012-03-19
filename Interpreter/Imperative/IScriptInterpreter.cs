using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.Interpreting
{
    public interface IScriptInterpreter
    {
        HashValue ExecuteScript(GlobalDataScope globalDataScope, BlockDataScope currentBlock, string scriptText);
    }
}