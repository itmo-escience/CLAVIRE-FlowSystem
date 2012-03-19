namespace Easis.Common.IdGenerators
{
    /// <summary>
    /// Интерфейс для генераторов идентификаторов.
    /// </summary>
    /// <typeparam name="TId">Тип генерируемого идентификатора.</typeparam>
    public interface IIdGenerator<out TId>
    {
        /// <summary>
        /// Возвращает следующий идентификатор.
        /// </summary>
        /// <returns>Новый идентификатор.</returns>
        TId GetId();
    }
}