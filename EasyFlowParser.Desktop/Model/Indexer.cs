using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс, хранящий индексатор.
    /// </summary>
    public class Indexer : ParsedObject
    {
        private Expression _index;

        /// <summary>
        /// Значение индекса.
        /// </summary>
        public Expression Index
        {
            get { return _index; }            
        }

        /// <summary>
        /// Конструктор с индексом.
        /// </summary>
        /// <param name="index">Индекс.</param>
        public Indexer(Expression index)
        {
            if (index == null) throw new ArgumentNullException("index");
            _index = index;
        }

        public override string ToString()
        {
            return _index.ToString();
        }

        protected override object CreateClone()
        {
            return new Indexer((Expression) _index.Clone());
        }
    }
}
