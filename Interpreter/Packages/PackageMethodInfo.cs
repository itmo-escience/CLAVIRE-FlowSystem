using System;
using System.Collections.Generic;
using Easis.Wfs.Interpreting.Utils;

namespace Easis.Wfs.Interpreting
{
    public sealed class PackageMethodInfo
    {
        private string _name;
        public string Name { get { return _name; } }

        private IDictionary<string, ParamInfo> _params = new Dictionary<string, ParamInfo>();
        public IEnumerable<ParamInfo> Params
        {
            get { return _params.Values; }
            set
            {
                _params = new Dictionary<string, ParamInfo>();
                foreach (var paramInfo in value)
                {
                    _params.Add(paramInfo.Name, paramInfo);
                }
            }
        }

        private IDictionary<string, ResultInfo> _results = new Dictionary<string, ResultInfo>();
        public IEnumerable<ResultInfo> Results
        {
            get { return _results.Values; }
            set
            {
                _results = new Dictionary<string, ResultInfo>();
                foreach (var resultInfo in value)
                {
                    _results.Add(resultInfo.Name, resultInfo);
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public PackageMethodInfo(string name)
        {
            _name = name;
        }

        public ParamInfo GetParamByName(string name)
        {
            if (_params.ContainsKey(name))
            {
                return _params[name];
            }
            else
            {
                throw new ObjectNotFoundException(String.Format("Method {0} doesn't contain param {1}", Name, name));
            }
        }
    }
}