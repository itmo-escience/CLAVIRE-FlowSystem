using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Easis.Wfs.Interpreting.Utils;

namespace Easis.Wfs.Interpreting
{
    public sealed class PackageInfo
    {
        private string _domain;
        public string Domain { get { return _domain; } }

        private string _name;
        public string Name { get { return _name; } }

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        private IDictionary<string, PackageMethodInfo> _methods = new Dictionary<string, PackageMethodInfo>();
        public ICollection<PackageMethodInfo> Methods
        {
            get { return _methods.Values; }
            set
            {
                _methods = new Dictionary<string, PackageMethodInfo>();
                foreach (var packageMethodInfo in value)
                {
                    _methods.Add(packageMethodInfo.Name,packageMethodInfo);
                }
            }
        }

        public PackageInfo(string domain, string name)
        {
            _domain = domain;
            _name = name;
        }
        public PackageMethodInfo GetMethodByName(string name)
        {
            if (_methods.ContainsKey(name))
            {
                return _methods[name];
            }
            else
            {
                throw new ObjectNotFoundException(String.Format("Package {0} doesn't contain method {1}", Name, name));
            }
        }
    }
}