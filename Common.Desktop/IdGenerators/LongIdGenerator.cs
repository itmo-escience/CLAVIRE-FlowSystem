using System.Threading;

namespace Easis.Common.IdGenerators
{
    /// <summary>
    /// Генератор идентификаторов типа long.
    /// </summary>
    public class LongIdGenerator : IIdGenerator<long>
    {
        private long _current;
        private object _lock = new object();

        /// <summary>
        /// Возвращает следующий идентификатор.
        /// </summary>
        /// <returns>Новый идентификатор.</returns>
        public long GetId()
        {
            lock (_lock) return _current++;
        }
    }
}