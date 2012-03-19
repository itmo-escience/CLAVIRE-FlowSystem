using System;
using Easis.Common;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Base class for typed values.
    /// </summary>
    public abstract class ValueBase: 
        ParsedObject,
        IEquatable<ValueBase>,
        IValueGetterEcx,
        IValueSetterEcx,
        ICloneable
    {
        private object _value;
        private DataType _dataType;

        /// <summary>
        /// Gets value.
        /// </summary>
        public virtual object Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets value data type.
        /// </summary>
        public DataType DataType
        {
            get { return _dataType; }
            protected set { _dataType = value; }
        }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        protected ValueBase()
        {
        }

        protected ValueBase(object val)
        {
            _value = val;
        }

        /// <summary>
        /// Returns whether this value can be cast to the passed data type.
        /// </summary>
        /// <param name="dataType">Data type to check.</param>
        /// <returns>Can / can't.</returns>
        public abstract bool CanCastTo(DataType dataType);

        /// <summary>
        /// Приводит данное значение к переданному типу.
        /// </summary>
        /// <param name="dataType">Тип, к которому осуществляется приведение.</param>
        /// <returns>Приведённое значение.</returns>
        public abstract ValueBase CastTo(DataType dataType);        

        public virtual void SetValue(CompoundVarIdentifier identifier, ValueBase value, IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (value == null) throw new ArgumentNullException("value");

            throw new InvalidOperationException(String.Format("Using {0} as a complex type", DataType));
        }

        public virtual ValueBase GetValue(CompoundVarIdentifier identifier, IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");

            if (identifier.NameParts.Count == 1)
                return this;

            throw new InvalidOperationException(String.Format("Using value {0} as a reference type while setting new value", DataType));
        }

       
        /// <summary>
        /// Возвращает, установлено ли значение.
        /// </summary>        
        public bool IsNull { get { return Value == null; } }

        public override string ToString()
        {
            return _value.ToString();
        }

        #region Equality members

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name = "other">An object to compare with this object.</param>
        public bool Equals(ValueBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._value, _value) && Equals(other._dataType, _dataType);
        }

        /// <summary>
        ///   Determines whether the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref = "T:System.Object" /> is equal to the current <see cref = "T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name = "obj">The <see cref = "T:System.Object" /> to compare with the current <see cref = "T:System.Object" />. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ValueBase)) return false;
            return Equals((ValueBase) obj);
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref = "T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return HashCode.Generate(_value, _dataType);
        }

        #endregion

        protected virtual object CloneValue()
        {
            return Value;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public virtual object Clone()
        {
            ValueBase clone = (ValueBase) CreateClone();
            clone._value = CloneValue();
            clone._dataType = _dataType;

            return clone;
        }

        protected override object CreateClone()
        {
            throw new InvalidOperationException();
        }
    }
}