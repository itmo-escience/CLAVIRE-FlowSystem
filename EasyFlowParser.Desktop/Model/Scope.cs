using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Область видимости кода.
    /// </summary>
    public class Scope : ParsedObject
    {
        private Scope _parent;
        private ScopeType _scopeType;
        private ScopeCollection _children;
        private Scope _rootScope = null;

        /// <summary>
        /// Возвращает тип области.
        /// </summary>
        public ScopeType ScopeType
        {
            get { return _scopeType; }            
        }

        /// <summary>
        /// Возвращает корневой элемент для данной области.
        /// </summary>
        public Scope RootScope
        {
            get { return _rootScope; }
            internal set { _rootScope = value; }
        }

        /// <summary>
        /// Возвращает, является ли данная область корневой.
        /// </summary>
        public bool IsRoot
        {
            get
            {
                return _parent == null;
            }
        }

        /// <summary>
        /// Родительская область видимости (может быть null, если это корень).
        /// </summary>
        public Scope Parent
        {
            get { return _parent; }
            internal set
            {
                if (value == _parent)
                    return;

                _parent = value;
            }
        }

        /// <summary>
        /// Коллекция дочерних областей видимости.
        /// </summary>
        public ScopeCollection Children
        {
            get { return _children; }            
        }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public Scope(ScopeType scopeType)
        {
            _children = new ScopeCollection(this);
            _scopeType = scopeType;
            _rootScope = this;
        }

        /// <summary>
        /// Определяет, находится ли данная область внутри области указанного типа.
        /// </summary>
        /// <param name="scopeType">Искомая область.</param>
        /// <param name="isInclusive">Включать ли в поиск саму область (this).</param>
        /// <returns>Находится / не находится.</returns>
        public bool IsInScope(ScopeType scopeType, bool isInclusive = true)
        {
            Scope current = isInclusive ? this : Parent;

            while (current != null) // Бежим вверх по родителям
            {
                if (current._scopeType == scopeType)
                    return true;

                current = current.Parent;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0}", _scopeType);
        }

        protected override object CreateClone()
        {
            return new Scope(ScopeType);
        }
    }
}
