using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Value of Structure type.
    /// </summary>
    public class StructureValue : ValueBase, IEquatable<StructureValue>
    {
        protected IDictionary<string, ValueBase> _fields = new Dictionary<string, ValueBase>();

        public IDictionary<string, ValueBase> AsDict
        {
            get { return _fields; }
        }

        protected void Add(string key, ValueBase value)
        {
            _fields.Add(key, value);
        }

        /// <summary>
        /// Возвращает или устанавливает поле структуры.
        /// При установке: если его нет - создает.
        /// </summary>
        /// <param name="name">Имя поля.</param>
        /// <returns>Значение поля.</returns>
        public ValueBase this[string name]
        {
            get
            {
                return _fields[name];
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _fields[name] = value;
            }
        }

        /// <summary>
        /// Проверяет, имеется ли в структуре поле с переданным именем.
        /// </summary>
        /// <param name="name">Имя искомого поля.</param>
        /// <returns>Имеется / не имеется.</returns>
        public bool HasField(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            return _fields.ContainsKey(name);
        }

        public StructureValue()
        {
            DataType = DataType.Structure;
        }

        public StructureValue(IEnumerable<KeyValuePair<string, ValueBase>> initialFields) : this()
        {
            if (initialFields == null) throw new ArgumentNullException("initialFields");

            if (initialFields.Any(pair => string.IsNullOrEmpty(pair.Key)))
                throw new EasyFlowException("One or more initial fields has empty or null name.");

            foreach (var pair in initialFields)
                _fields[pair.Key] = pair.Value;
        }

        /// <summary>
        /// Возвращает, может ли данное значение быть приведено к переданному типу.
        /// </summary>
        /// <param name="dataType">Тип.</param>
        /// <returns>Может / не может.</returns>
        public override bool CanCastTo(DataType dataType)
        {
            return false;
        }

        /// <summary>
        /// Приводит данное значение к переданному типу.
        /// </summary>
        /// <param name="dataType">Тип, к которому осуществляется приведение.</param>
        /// <returns>Приведённое значение.</returns>
        public override ValueBase CastTo(DataType dataType)
        {
            throw new NotSupportedException(String.Format("Can't cast Int to {0}.", dataType));
        }

        #region Equality members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(StructureValue other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other._fields, _fields);
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
            return Equals(obj as StructureValue);
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
            unchecked
            {
                // TODO: проверить, нормально ли реализован GetHashCode у KeyValuePair.
                return HashCode.Combine(HashCode.Generate(_fields), base.GetHashCode());
            }
        }

        #endregion

        public override ValueBase GetValue(CompoundVarIdentifier identifier,IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");

            if (identifier.NameParts.Count == 0)
                throw new InvalidOperationException("Check compoundvar logic in structure get");

            // имя не надо сравнивать, так-как могут быть анонимные структуры полученные из индексера
            var first = identifier.NameParts[0];

            if (first.HasIndexer)
                throw new InvalidOperationException("Structure can't have indexer");

            // обращение к самой структуре
            if (identifier.NameParts.Count == 1)
                return this;

            // Далее - обращение к полю

            var fieldName = identifier.NameParts[1].Name;

            if (HasField(fieldName))
            {
                return this[fieldName].GetValue(identifier.SubIdentifier(1),evaluationContext);
            }
            else
            {
                //TODO: создать тип исключения)
                throw new InvalidOperationException(String.Format("Field '{0}' is undefined in structure '{1}' (fields: {2}).", fieldName, first, String.Join(", ", _fields.Keys)));
            }
        }

        public override void SetValue(CompoundVarIdentifier identifier, ValueBase value,IEvaluationContext evaluationContext)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (value == null) throw new ArgumentNullException("value");

            if (identifier.NameParts.Count == 0)
            {
                throw new InvalidOperationException("Check compoundvar logic in structure get");
            }

            // имя не надо сравнивать, так-как могут быть анонимные структуры полученные из индексера
            var first = identifier.NameParts[0];

            if (first.HasIndexer)
            {
                throw new InvalidOperationException("Structure can't have indexer");
            }

            // обращение к самой структуре
            if (identifier.NameParts.Count == 1)
            {
                throw new InvalidOperationException(String.Format("Using value {0} as a reference type while setting new value", DataType));
            }

            var fieldName = identifier.NameParts[1].Name;

            if (HasField(fieldName))
            {
                // установить значение имеющегося поля
                this[fieldName] = value;
            }
            else
            {
                //создать новое поле
                Add(fieldName, value);
            }
        }

        protected override object CloneValue()
        {
            Dictionary<string, ValueBase> clone = new Dictionary<string, ValueBase>();

            foreach (var valuePair in _fields)
            {
                clone[valuePair.Key] = (ValueBase) valuePair.Value.Clone();
            }

            return clone;
        }

        protected override object CreateClone()
        {
            return new StructureValue();
        }

    }
}
