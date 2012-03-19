#region

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Easis.Wfs.EasyFlow.Utils
{
	/// <summary>
	/// Позволяет создать индексированное свойство для класса с целью доступа
	/// к элементам внутреннего Dictionary
	/// </summary>
	/// <remarks>
	/// Пример использования:
	/// <example>
	/// <code>
	/// <![CDATA[
	/// 
	///		class SomeClass
	///		{
	///			private Dictionary<string, int> things = new Dictionary<string, int>();
	///			
	///			public DictionaryIndexedProperty<string, int> Things
	///			{
	///				get
	///				{
	///					return new DictionaryIndexedProperty<string, int>(things);
	///				}
	///			}
	///		}
	///		
	///		SomeClass obj = new SomeClass();
	///		string key = "foo";
	///		int value = obj.Things[key];
	///	]]/>
	/// </code>
	/// </example>
	/// </remarks>
	/// <typeparam name="TKey">Тип ключа</typeparam>
	/// <typeparam name="TValue">Тип значения</typeparam>
	public class DictionaryIndexedProperty<TKey, TValue> : CollectionIndexedProperty<TKey, TValue>,
	                                                       IEnumerable<KeyValuePair<TKey, TValue>>
	{
		private Dictionary<TKey, TValue> _dict;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="dict">Словарь, к которому требуется предоставить доступ</param>
		public DictionaryIndexedProperty(Dictionary<TKey, TValue> dict, bool isReadOnly = true) : base(isReadOnly)
		{
			this._dict = dict;
		}

		/// <summary>
		/// Индексированное свойство
		/// </summary>
		/// <param name="index">Ключ словаря</param>
		/// <returns>Значение по ключу</returns>
		public override TValue this[TKey index]
		{
			get
			{
				lock (_dict)
				{
					return _dict[index];
				}
			}

            set
            {
                lock (_dict)
                {
                    if (IsReadOnly)
                        throw new InvalidOperationException("Нельзя присвоить: словарь только для чтения.");

                    _dict[index] = value;
                }
            }
		}

		/// <summary>
		/// Коллекция значений
		/// </summary>
		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				lock (_dict)
				{
					return _dict.Values;
				}
			}
		}

		/// <summary>
		/// Коллекция ключей
		/// </summary>
		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				lock (_dict)
				{
					return _dict.Keys;
				}
			}
		}

		public override int Count
		{
			get
			{
				lock (_dict)
				{
					return _dict.Count;
				}
			}
		}

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			lock (_dict)
			{
				return _dict.GetEnumerator();
			}
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			lock (_dict)
			{
				return _dict.GetEnumerator();
			}
		}

		#endregion

		public override bool ConatainsKey(TKey key)
		{
			lock (_dict)
			{
				return _dict.ContainsKey(key);
			}
		}

		public override bool Contains(TValue value)
		{
			lock (_dict)
			{
				return _dict.ContainsValue(value);
			}
		}
	}
}