using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    public class IdentifierTable
    {
        private Dictionary<string, Identifier> _ids = new Dictionary<string, Identifier>();

        public IdentifierTable()
        {
        }

        public bool HasIdentifier(string name)
        {
            return _ids.ContainsKey(name);
        }

        public Identifier this[string idName]
        {
            get
            {
                if (!HasIdentifier(idName))
                    throw new KeyNotFoundException(string.Format("Идентификатор {0} не найден в таблице идентификаторов.", idName));

                return _ids[idName];
            }

            set
            {

                _ids[value.Name] = value;
            }
        }        

        public void Add(Identifier identifier)
        {            
            _ids.Add(identifier.Name, identifier);
        }        
    }
}
