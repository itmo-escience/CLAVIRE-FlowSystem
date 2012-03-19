using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс, хранящий составное имя объекта.
    /// </summary>
    public class RunObjectName : StringCompoundName
    {
        public RunObjectName()
        {
        }


        public RunObjectName(string str) : base(str)
        {

        }

        public RunObjectName(CompoundName<string> name): base(name)
        {
        }

        public override bool CheckPart(string part)
        {
            Regex regex = new Regex(@"^[\w\-]+");            

            return regex.IsMatch(part);
        }

        protected override object CreateClone()
        {
            return new RunObjectName();
        }
    }
}
