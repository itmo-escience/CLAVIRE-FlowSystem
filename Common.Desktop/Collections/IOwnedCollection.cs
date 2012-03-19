using System.Collections.Generic;

namespace Easis.Common.Collections
{
    /// <summary>
    /// Интерфейс коллекции, которой кто-то владеет. Причём объект-владелец при добавлении
    /// в коллекцию каким-то образом модифицирует объект (например, проставляет ему
    /// Id), поэтому такая коллекция не может содержать пустые и дублирующие элементы.
    /// Классы, реализующие этот интерфейс, должны это учитывать.
    /// </summary>
    /// <typeparam name="TOwner">Тип владельца коллекции.</typeparam>
    /// <typeparam name="TItem">Тип элемента коллекции.</typeparam>
    public interface IOwnedCollection<out TOwner, TItem> : IList<TItem>
    {
        /// <summary>
        /// Возвращает владельца коллекции.
        /// </summary>
        TOwner Owner { get; }
    }
}
