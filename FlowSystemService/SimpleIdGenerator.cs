using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Easis.Wfs.FlowSystemService
{
    /// <summary>
    /// Thread safe id generator
    /// </summary>
    public class SimpleIdGenerator : IIdGenerator<int>
    {
        private int _current;

        public SimpleIdGenerator()
        {
            lock (this)
                _current = -1;
        }

        public int NextId()
        {
            int ret = -1;
            lock (this)
                ret = ++_current;
            return ret;
        }
    }
}