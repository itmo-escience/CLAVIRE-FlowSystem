using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easis.Wfs.FlowSystemService.Utils
{
    public class Pair<F, S>
    {
        private F _first;
        private S _second;

        public Pair(F first, S second)
        {
            _first = first;
            _second = second;
        }

        public F First
        {
            get { return _first; }
        }

        public S Second
        {
            get { return _second; }
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj is Pair<F, S>)
        //    {
        //        var t = obj as Pair<F, S>;
        //        return First == t.First && Second == t.Second;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}