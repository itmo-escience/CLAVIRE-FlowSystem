namespace Easis.Wfs.EasyFlow.Utils
{
	/// <summary>
	/// Класс для создания индексированных свойств для доступа read-only ко внутренным
	/// объектам по ключу
	/// </summary>
	/// <typeparam name="TKey">Тип ключа</typeparam>
	/// <typeparam name="TValue">Тип значения</typeparam>
	public abstract class IndexedProperty<TKey, TValue>
	{
        private bool isReadOnly;

	    protected IndexedProperty(bool isReadOnly)
	    {
	        this.isReadOnly = isReadOnly;
	    }

	    public bool IsReadOnly
	    {
	        get { return isReadOnly; }
	    }

	    /// <summary>
		/// Свойство-индексатор по ключу
		/// </summary>
		/// <param name="index">Ключ</param>
		/// <returns>Значение</returns>
        public abstract TValue this[TKey index] { get; set; }
	}
}