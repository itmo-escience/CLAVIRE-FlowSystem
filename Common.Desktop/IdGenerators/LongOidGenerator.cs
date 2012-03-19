using System;
using System.Collections.Generic;
using System.Linq;

namespace Easis.Common.IdGenerators
{
    /// <summary>
    /// Генератор идентификаторов типа long для объектов.
    /// Thread-safe
    /// </summary>
    public class LongOidGenerator : OidGeneratorBase<long>
    {
        private IIdGenerator<long> _longIdGenerator = new LongIdGenerator();

        protected override long GenerateId(object obj)
        {
            return _longIdGenerator.GetId();
        }
    }
}