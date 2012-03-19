using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Блок для конструкции For.
    /// </summary>
    public class ForBlock : BlockBase
    {
        protected override object CreateClone()
        {
            return new ForBlock();
        }
    }
}
