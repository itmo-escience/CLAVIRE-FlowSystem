using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Common.IdGenerators
{
    /// <summary>
    /// Интерфейс для генерации идентификаторов для объектов.
    /// Для одного и того же объект должен возвращаться всегда один
    /// и тот же Id независимо от того, сколько раз вызван метод
    /// GetId().
    /// </summary>
    /// <typeparam name="TId">Тип генерируемого идентификатора.</typeparam>
    public interface IOidGenerator<out TId>
    {
        /// <summary>
        /// Возвращает идентификатор для переданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Идентификатор.</returns>
        TId GetId(object obj);
    }
}
