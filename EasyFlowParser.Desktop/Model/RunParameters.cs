using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    public class RunParameters : ParsedObject, ICloneable
    {
        public NamedParameterCollection Parameters { get; internal set; }

        public RunParameters()
        {
            Parameters = new NamedParameterCollection();
        }

        public RunParameters(IEnumerable<NamedParameter> parameters)
        {
            Parameters = new NamedParameterCollection(parameters);
        }

        public void AddRange(IEnumerable<NamedParameter> parameters)
        {
            Parameters.AddRange(parameters);   
        }

        protected override object CreateClone()
        {
            return new RunParameters((IEnumerable<NamedParameter>)Parameters.Clone());
        }

        public NamedParameter this[string name]
        {
            get { return Parameters.First(parameter => parameter.Name == name); }
        }

    }
}
