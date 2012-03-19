namespace Easis.Wfs.EasyFlow.Utils
{
	/// <summary>
	/// Предок для индексированных свойств, основанных на коллекциях
	/// </summary>
	/// <typeparam name="TKey">Тип ключа</typeparam>
	/// <typeparam name="TValue">Тип значения</typeparam>
	public abstract class CollectionIndexedProperty<TKey, TValue> : IndexedProperty<TKey, TValue>
	{
	    protected CollectionIndexedProperty(bool isReadOnly = true): base(isReadOnly)
	    {
	    }

	    /// <summary>
		/// Количество элементов
		/// </summary>
		public abstract int Count { get; }

		/// <summary>
		/// Указывает, существует ли элемент с данным ключом
		/// </summary>
		/// <param name="key"></param>
		/// <returns>Существование</returns>
		public abstract bool ConatainsKey(TKey key);

		/// <summary>
		/// Указывает, существует ли элемент с данным значением
		/// </summary>
		/// <param name="value">Значение</param>
		/// <returns>Существование</returns>
		public abstract bool Contains(TValue value);
	}
}