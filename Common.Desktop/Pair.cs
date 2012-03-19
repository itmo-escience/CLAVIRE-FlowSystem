using System;
using System.Collections.Generic;

namespace Easis.Common
{
    /// <summary>
    /// Класс, хранящий пару значений.
    /// </summary>
    /// <typeparam name="TFirst">Тип первого элемента.</typeparam>
    /// <typeparam name="TSecond">Тип второго элемента.</typeparam>
    public struct Pair<TFirst, TSecond> : IEquatable<Pair<TFirst, TSecond>>, IEquatable<KeyValuePair<TFirst, TSecond>>
    {
        private TFirst _first;
        private TSecond _second;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="first">Первое значение.</param>
        /// <param name="second">Второе значение.</param>
        public Pair(TFirst first, TSecond second)
        {
            _first = first;
            _second = second;
        }

        /// <summary>
        /// Копирующий конструктор.
        /// </summary>
        /// <param name="pair">Пара-донор.</param>
        public Pair(Pair<TFirst, TSecond> pair)
        {
            _first = pair._first;
            _second = pair._second;
        }

        /// <summary>
        /// Возвращает первое значение.
        /// </summary>
        public TFirst First
        {
            get { return _first; }
        }

        /// <summary>
        /// Возвращает второе значение.
        /// </summary>
        public TSecond Second
        {
            get { return _second; }
        }

        #region Equality members
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Pair<TFirst, TSecond> other)
        {
            return Equals(other._first, _first) && Equals(other._second, _second);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(KeyValuePair<TFirst, TSecond> other)
        {
            return Equals(other.Key, _first) && Equals(other.Value, _second);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Pair<TFirst, TSecond>)) return false;
            return Equals((Pair<TFirst, TSecond>) obj);
        }
        
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return HashCode.Generate(_first, _second);
            }
        }
        #endregion
    }
}