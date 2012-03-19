using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Common.IdGenerators
{
    public class StatelessOidGenerator<TId> : OidGeneratorBase<TId>
    {
        private Func<object, TId> _genFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public StatelessOidGenerator(Func<object, TId> genFunc)
        {
            if (genFunc == null) throw new ArgumentNullException("genFunc");
            _genFunc = genFunc;
        }        

        /// <summary>
        /// Реализация дочерний классов должна по переданному объекту возвращать Id.
        /// </summary>
        /// <param name="obj">Объект, для которого запрашивается идентификатор.</param>
        /// <returns>Идентификатор.</returns>
        protected override TId GenerateId(object obj)
        {            
            return _genFunc.Invoke(obj);
        }
    }
}
