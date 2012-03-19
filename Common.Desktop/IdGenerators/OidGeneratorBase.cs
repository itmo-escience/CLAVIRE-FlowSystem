using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Common.IdGenerators
{
    /// <summary>
    /// Базовый абстрактный генератор идентификаторов для объектов. (thread-safe).
    /// </summary>
    /// <typeparam name="TId">Тип генерируемого идентификатора.</typeparam>
    public abstract class OidGeneratorBase<TId> : IOidGenerator<TId>
    {
        private object _getIdLock = new object();                

        private List<Pair<object, TId>> _entries = new List<Pair<object, TId>>();

        /// <summary>
        /// Реализация дочерних классов должна по переданному объекту возвращать Id.
        /// </summary>
        /// <param name="obj">Объект, для которого запрашивается идентификатор.</param>
        /// <returns>Идентификатор.</returns>
        protected abstract TId GenerateId(object obj);        

        /// <summary>
        /// Возвращает идентификатор для объекта. Для одного и того же объекта
        /// при двух последовательных вызовах вернётся один и тот же идентфикатор.
        /// </summary>
        /// <param name="obj">Объект, для которого запрашивается идентификатор.</param>
        /// <returns>Идентификатор.</returns>
        public TId GetId(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            lock (_getIdLock)
            {
                if (_entries.Any(pair => ReferenceEquals(pair.First, obj))) // если такой объект уже был            
                    return _entries.Single(pair => ReferenceEquals(pair.First, obj)).Second;

                // если объекта не было, генерируем новый Id
                TId newId = GenerateId(obj);
                _entries.Add(new Pair<object, TId>(obj, newId));

                return newId;
            }
        }
    }
}