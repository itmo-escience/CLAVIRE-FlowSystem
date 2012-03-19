using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Easis.Common;
using Easis.Wfs.EasyFlow.Utils;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Class for holding compound names.
    /// </summary>
    public abstract class CompoundName<TPart> : ParsedObject, IEquatable<CompoundName<TPart>>, IEnumerable<TPart>
    {
        private List<TPart> _nameParts = new List<TPart>();
        private string _delimeter = ".";
        private int _maxPartsNumber;

        /// <summary>
        /// Максимальное количество частей, после которого возникает ошибка.
        /// </summary>
        public int MaxPartsNumber
        {
            get { return _maxPartsNumber; }
            set { _maxPartsNumber = value; }
        }

        /// <summary>
        /// Разделитель элементов имени.
        /// </summary>
        public string Delimeter
        {
            get { return _delimeter; }
            set { _delimeter = value; }
        }

        /// <summary>
        /// Возвращает коллекцию только для чтения, хранящую части имени.
        /// </summary>
        public ListIndexedProperty<TPart> NameParts
        {
            get { return new ListIndexedProperty<TPart>(_nameParts); }
        }

        /// <summary>
        /// Проверяет, является ли добавляемая часть имени корректной.
        /// </summary>
        /// <param name="part">Часть имени.</param>
        /// <returns>Корректна / не корректна.</returns>
        public virtual bool CheckPart(TPart part)
        {            
            if (part.Equals(null)) throw new ArgumentNullException("part");

            return true;
        }

        /// <summary>
        /// Проверяет, можно ли добавить к имени ещё одну часть.
        /// </summary>
        /// <returns></returns>
        public bool CanAdd()
        {
            return _maxPartsNumber == 0 || _nameParts.Count < _maxPartsNumber;
        }

        /// <summary>
        /// Добавляет часть имени.
        /// </summary>
        /// <param name="part">Часть имени</param>
        public void AddPart(TPart part)
        {
            if (part.Equals(null)) throw new ArgumentNullException("part");

            if (!CheckPart(part))
                throw new FormatException(string.Format("Часть составного имения ({0}) имеет некорректный формат.", part));

            if (!CanAdd())
                throw new InvalidOperationException(string.Format("Количество компонент данного составного имени должно быть <= {0}.", _maxPartsNumber));
                    
            _nameParts.Add(part);
        }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        protected CompoundName()
        {
        }

        /// <summary>
        /// Конструктор со строковым представлением локатора.
        /// </summary>
        /// <param name="str">Строковое представление.</param>
        protected CompoundName(string str)
        {
            if (str == null) throw new ArgumentNullException("str");
            Parse(str);
        }

        /// <summary>
        /// Копирующий конструктор.
        /// </summary>
        /// <param name="name">Локатор-источник.</param>
        protected CompoundName(CompoundName<TPart> name)
        {
            Func<string, bool> check = null;

            TPart errorPart;

            if (null != (errorPart = name._nameParts.FirstOrDefault(part => !CheckPart(part))))
                throw new FormatException(string.Format("Ошибочная часть имени: {0}.", errorPart));

            AddParts(name._nameParts);
            _delimeter = name._delimeter;
        }

        /// <summary>
        /// Добавляет несколько частей к имени.
        /// </summary>
        /// <param name="parts">Перечисление с частями имени.</param>
        public void AddParts(IEnumerable<TPart> parts)
        {
            if (parts == null) throw new ArgumentNullException("parts");
            foreach(var part in parts)
                AddPart(part);
        }

        /// <summary>
        /// Очищает объект.
        /// </summary>
        public void Clear()
        {
            _nameParts.Clear();
        }

        /// <summary>
        /// Парсит части имени по переданной строке. Старые части очищаются.
        /// </summary>
        /// <param name="str">Составное имя.</param>                
        protected void Parse(string str)
        {
            if (str == null) throw new ArgumentNullException("str");

            Clear();
            AddParts(Split(str));            
        }

        /// <summary>
        /// Бьёт строку согласно определённым правилам.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TPart> Split(string str)
        {
            string[] parts = str.Split(_delimeter.ToCharArray());

            List<TPart> resultParts = new List<TPart>();

            foreach (var part in parts)            
                resultParts.Add(MakePart(part));           

            return resultParts;
        }

        /// <summary>
        /// Возвращает по переданной части имени его объектное представление.
        /// </summary>
        /// <param name="part">Часть имени.</param>
        /// <returns>Объектное представление.</returns>
        protected abstract TPart MakePart(string part);

        #region Equals memebers
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (CompoundName<TPart>)) return false;

            return Equals((CompoundName<TPart>) obj);
        }

        public bool Equals(CompoundName<TPart> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return other._nameParts.SequenceEqual(_nameParts);
        }

        public override int GetHashCode()
        {            
            return HashCode.Combine(
                            HashCode.Generate(_nameParts),
                            HashCode.Generate(_delimeter, _maxPartsNumber)
                        );
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<TPart> GetEnumerator()
        {
            return _nameParts.GetEnumerator();
        }

        public override string ToString()
        {
            return _nameParts.Aggregate("", (s, part) => s + (s == "" ? "" : _delimeter) + part.ToString());
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = (CompoundName<TPart>)base.Clone();

            clone._delimeter = _delimeter;
            clone._maxPartsNumber = _maxPartsNumber;            
            clone._nameParts = new List<TPart>(_nameParts);

            return clone;
        }
    }
}
