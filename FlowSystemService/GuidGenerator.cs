using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easis.Wfs.FlowSystemService
{
    public class GuidGenerator : IIdGenerator<Guid>
    {
        public Guid NextId()
        {
            return Guid.NewGuid();
        }
    }
}