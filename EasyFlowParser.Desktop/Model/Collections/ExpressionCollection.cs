using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model.Collections
{
    ///<summary>
    ///</summary>
    public class ExpressionCollection : ParsedCollection<Expression>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ExpressionCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ExpressionCollection(IEnumerable<Expression> items): base(items)
        {
        }
    }
}
