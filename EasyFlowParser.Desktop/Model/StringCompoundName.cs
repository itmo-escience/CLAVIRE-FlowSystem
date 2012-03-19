using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    public class StringCompoundName : CompoundName<string>
    {
        public StringCompoundName()
        {
        }

        public StringCompoundName(CompoundName<string> name): base(name)
        {
        }

        protected override string MakePart(string part)
        {
            return part;
        }

        public override bool CheckPart(string part)
        {
            return !part.Contains(".");
        }

        public StringCompoundName(string str) : base(str)
        {
        }

        protected override object CreateClone()
        {
            return new StringCompoundName();
        }
    }

}
