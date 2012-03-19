using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Коллекция именованных параметров.
    /// </summary>
    public class NamedParameterCollection : ParsedCollection<NamedParameter>, ICloneable
    {
        public NamedParameterCollection()
        {
        }

        public NamedParameterCollection(IEnumerable<NamedParameter> itemCollection): base(itemCollection)
        {
        }

        public object Clone()
        {
            return new NamedParameterCollection(this.Select(parameter => (NamedParameter)parameter.Clone()));
        }
    }
}
