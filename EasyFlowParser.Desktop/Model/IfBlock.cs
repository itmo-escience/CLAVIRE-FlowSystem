using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    public class IfBlock : BlockBase
    {
        public IfBlock()
        {
        }

        protected override object CreateClone()
        {
            return new IfBlock();
        }
    }
}
