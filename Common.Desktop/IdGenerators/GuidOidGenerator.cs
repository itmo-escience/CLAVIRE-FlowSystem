using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Common.IdGenerators
{
    public class GuidOidGenerator : OidGeneratorBase<Guid>
    {
        /// <summary>
        /// Реализация дочерний классов должна по переданному объекту возвращать Id.
        /// </summary>
        /// <param name="obj">Объект, для которого запрашивается идентификатор.</param>
        /// <returns>Идентификатор.</returns>
        protected override Guid GenerateId(object obj)
        {
            return Guid.NewGuid();
        }
    }
}
