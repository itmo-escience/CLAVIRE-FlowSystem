using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    public enum IdentifierType
    {
        /// <summary>
        /// Неопределённый.
        /// </summary>
        Undefined,
        /// <summary>
        /// Имя шага.
        /// </summary>
        StepName,
        /// <summary>
        /// Имя переменной.
        /// </summary>
        VarName,
        /// <summary>
        /// Имя указателя на файл.
        /// </summary>
        FilePointerName
    }

    
    /// <summary>
    /// Класс, представляющий собой идентификатор.
    /// </summary>
    public class Identifier : ParsedObject, IEquatable<Identifier>
    {
        private string _name;
        private IdentifierType _identifierType = IdentifierType.Undefined;

        /// <summary>
        /// Имя идентификатора.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Тип идентификатора.
        /// </summary>
        public IdentifierType IdentifierType
        {
            get { return _identifierType; }
            internal set { _identifierType = value; }
        }

        public Identifier(string name)
        {
            _name = name;            
        }

        /// <summary>
        /// Конструктор с именем.
        /// </summary>
        /// <param name="name">Имя идентификатора.</param>
        public Identifier(string name, IdentifierType identifierType)
        {
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
            _identifierType = identifierType;
        }

        public override string ToString()
        {
            return _name;
        }

        public bool Equals(Identifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._name, _name) && Equals(other._identifierType, _identifierType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Identifier)) return false;
            return Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_name != null ? _name.GetHashCode() : 0)*397) ^ _identifierType.GetHashCode();
            }
        }

        protected override object CreateClone()
        {
            return new Identifier(_name, _identifierType);
        }
    }
}
