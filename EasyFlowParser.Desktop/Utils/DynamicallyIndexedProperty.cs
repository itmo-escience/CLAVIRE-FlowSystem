#region

using System;
using System.Threading;

#endregion

namespace Easis.Wfs.EasyFlow.Utils
{
	public delegate TValue SelectDelegate<TKey, TValue>(TKey key);

	/// <summary>
	/// Динамически индексируемое свойство
	/// </summary>
	/// <typeparam name="TKey">Тип ключа</typeparam>
	/// <typeparam name="TValue">Тип значения</typeparam>
	public class DynamicallyIndexedProperty<TKey, TValue> : IndexedProperty<TKey, TValue>
	{
		/// <summary>
		/// Делегат выбра элементов по ключу
		/// </summary>
		private SelectDelegate<TKey, TValue> _selectDelegate;

		/// <summary>
		/// Конструктор с делегатом выбора по ключу
		/// </summary>
		/// <param name="selectDelegate">Делегат выбора по ключу</param>
		public DynamicallyIndexedProperty(SelectDelegate<TKey, TValue> selectDelegate): base(true)
		{
			if (selectDelegate == null) throw new ArgumentNullException("selectDelegate");

			_selectDelegate = selectDelegate;
		}		

		public override TValue this[TKey index]
		{
			get { return _selectDelegate(index); }
            set { throw new InvalidOperationException("Невозможно присваивать динамически индексируемуму свойству.");  }
		}
	}
}