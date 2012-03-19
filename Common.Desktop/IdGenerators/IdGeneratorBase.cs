using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Common.IdGenerators
{
    public abstract class IdGeneratorBase<TId> : IIdGenerator<TId>
    {
        public abstract TId GetId();
    }
}
