using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common.Collections;

namespace Easis.Wfs.EasyFlow.Parsing
{
    public class ParserMessageCollection : ListCollection<ParserMessage>
    {                
        public IEnumerable<ParserMessage> Filter(ParserMessageType [] types)
        {
            return this.Where(message => types.Contains(message.MessageType));
        }
    }
}
