using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model.Expressions
{
    public static class ConstHolder
    {
        private static Dictionary<string, ValueBase> _consts;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static ConstHolder()
        {
            _consts = new Dictionary<string, ValueBase>
                {
                    {"raw", new StringValue("raw")},
                    {"meta", new StringValue("meta")},
                    {"low", new StringValue("low")},
                    {"normal", new StringValue("normal")},
                    {"high", new StringValue("high")},
                    {"urgent", new StringValue("urgent")},
                    
                    {"zip", new StringValue("zip")},
                    {"decart", new StringValue("decart")},
                };
        }

        public static bool HasConst(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            return _consts.ContainsKey(name);
        }

        public static ValueBase GetConst(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            return _consts[name];
        }

        public static void SetConst(string name, ValueBase value)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");

            _consts[name] = value;
        }
    }
}
