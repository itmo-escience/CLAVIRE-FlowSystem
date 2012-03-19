using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс, хранящий доступ к простой переменной (по имени или по имени и индексатору)
    /// </summary>
    public class SimpleVarIdentifier : ParsedObject
    {
        private string _name;
        private Indexer _indexer = null;

        /// <summary>
        /// Имя переменной.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Индексатор (null, если нет).
        /// </summary>
        public Indexer Indexer
        {
            get { return _indexer; }
        }

        /// <summary>
        /// Имеется ли индексатор.
        /// </summary>
        public bool HasIndexer
        {
            get { return _indexer != null; }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя переменной.</param>
        /// <param name="indexer">Индексатор переменной.</param>
        public SimpleVarIdentifier(string name, Indexer indexer = null)
        {
            if (name == null) throw new ArgumentNullException("name");

            _name = name;
            _indexer = indexer;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", _name, HasIndexer ? "[" + _indexer + "]" : "");
        }

        protected override object CreateClone()
        {
            return new SimpleVarIdentifier(_name, HasIndexer ? (Indexer)_indexer.Clone() : null);
        }
    }
}
