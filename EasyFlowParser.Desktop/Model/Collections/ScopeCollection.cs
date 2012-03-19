using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Коллекция областей видимости.
    /// </summary>
    public class ScopeCollection : ParsedOwnedCollection<Scope, Scope>
    {
        /// <summary>
        /// Конструктор с родителем.
        /// </summary>
        /// <param name="parent">Родитель коллекции.</param>
        public ScopeCollection(Scope parent): base(parent)
        {
        }

        /// <summary>
        /// Копирующий конструктор с родителем.
        /// </summary>
        /// <param name="parent">Родитель коллекции.</param>
        /// <param name="itemCollection">Элементы коллекции.</param>
        public ScopeCollection(Scope parent, IEnumerable<Scope> itemCollection): base(parent, itemCollection)
        {
        }

        protected override void AdoptItem(Scope item)
        {
            item.Parent = Owner;
            item.RootScope = Owner.RootScope;
        }

        protected override void DeadoptItem(Scope item)
        {
            item.Parent = null;
            item.RootScope = item;
        }
    }
}
