#region

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Easis.Wfs.EasyFlow.Utils
{
	/// <summary>
	/// Класс для создания индексированных свойств для доступа read-only ко внутренным
	/// объектам типа List
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	public class ListIndexedProperty<TValue> : CollectionIndexedProperty<int, TValue>, IEnumerable<TValue>
	{
		private List<TValue> _list;


	    public ListIndexedProperty(List<TValue> list, bool isReadOnly = true): base(isReadOnly)
	    {
	        if (list == null) throw new ArgumentNullException("list");
	        _list = list;
	    }

	    public ListIndexedProperty()
	    {
	    }

	    /// <summary>
		/// Количество элементов
		/// </summary>
		public override int Count
		{
			get
			{
				int count;

				lock (_list)
				{
					count = _list.Count;
				}

				return count;
			}
		}

		/// <summary>
		/// Возвращает элемент списка по индексу
		/// </summary>
		/// <param name="index">Индекс</param>
		/// <returns></returns>
		public override TValue this[int index]
		{
			get
			{
				lock (_list)
				{
					return _list[index];
				}
			}

            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("Нельзя присвоить: список только для чтения.");

                lock (_list)
                {
                    _list[index] = value;
                }
            }
		}

		#region IEnumerable<TValue> Members

		public IEnumerator<TValue> GetEnumerator()
		{
			lock (_list)
			{
				return _list.GetEnumerator();	
			}			
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			lock (_list)
			{
				return _list.GetEnumerator();
			}
		}

		#endregion

		/// <summary>
		/// Указывает, существует ли элемент с данным значением
		/// </summary>
		/// <param name="value">Значение</param>
		/// <returns>Существование</returns>
		public override bool Contains(TValue value)
		{
			lock (_list)
			{
				return _list.Contains(value);
			}
		}

		/// <summary>
		/// Указывает, существует ли элемент с данным ключом
		/// </summary>
		/// <param name="key"></param>
		/// <returns>Существование</returns>
		public override bool ConatainsKey(int key)
		{
			lock (_list)
			{
				return _list.Count > key;	
			}			
		}
	}
}