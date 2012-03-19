using Easis.Wfs.EasyFlow.Model;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.Interpreting
{
    public sealed class ParamInfo
    {
        public string Name { get; set; }
        public DataType DataType { get; set; }
        public bool Required { get; set; }
    }
}