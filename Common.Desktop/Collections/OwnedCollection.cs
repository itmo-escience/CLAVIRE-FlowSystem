using System;
using System.Collections.Generic;

namespace Easis.Common.Collections
{
    /// <summary>
    /// Класс коллекции, которой кто-то владеет.
    /// Причём объект-владелец при добавлении в коллекцию каким-то образом модифицирует
    /// объект (например, проставляет ему Id), поэтому такая коллекция не может содержать
    /// пустые и дублирующие элементы.
    /// </summary>
    /// <typeparam name="TOwner">Тип владельца коллекции.</typeparam>
    /// <typeparam name="TItem">Тип элемента коллекции.</typeparam>
    public class OwnedCollection<TOwner, TItem> : ListCollection<TItem>, IOwnedCollection<TOwner, TItem>
    {
        private TOwner _owner;

        private bool IsNull(TItem item)
        {
            return ReferenceEquals(item, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public OwnedCollection(TOwner owner)
        {
            if (ReferenceEquals(owner, null))                
                throw new ArgumentNullException("owner");

            _owner = owner;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public OwnedCollection(TOwner owner, IEnumerable<TItem> items) : this(owner)
        {
            if (items == null) throw new ArgumentNullException("items");
            AddRange(items);
        }

        protected virtual void AdoptItem(TItem item)
        {
            // Intentionally left blank
        }

        protected virtual void DeadoptItem(TItem item)
        {
            // Intentionally left blank
        }


        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public override void Add(TItem item)
        {            
            if (IsNull(item)) throw new ArgumentNullException("item");

            lock (SyncRoot)
            {
                CheckDuplicatie(item);
                base.Add(item);
                AdoptItem(item);    
            }      
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public override void Clear()
        {
            lock (SyncRoot)
            {
                IList<TItem> temp = new List<TItem>(this);
                base.Clear();
                foreach (TItem item in temp)
                    DeadoptItem(item);            
            }
        }


        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public override bool Remove(TItem item)
        {
            if (IsNull(item)) throw new ArgumentNullException("item");

            lock (SyncRoot)
            {
                bool isOk = base.Remove(item);
                if (isOk) DeadoptItem(item);
                return isOk;
            }
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public override void Insert(int index, TItem item)
        {
            if (IsNull(item)) throw new ArgumentNullException("item");

            lock (SyncRoot)
            {
                base.Insert(index, item);
                AdoptItem(item);
                CheckDuplicatie(item);
            }            
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public override void RemoveAt(int index)
        {
            lock (SyncRoot)
            {
                TItem removedItem = base[index];
                base.RemoveAt(index);
                DeadoptItem(removedItem);    
            }
        }

        private void CheckDuplicatie(TItem item)
        {
            if (Contains(item))
                throw new ArgumentException("Such element already exists in the owned list.");
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public override TItem this[int index]
        {
            get { return base[index]; }
            set
            {
                lock (SyncRoot)
                {
                    if (IsNull(value)) throw new ArgumentNullException("value");

                    if (Contains(value) && IndexOf(value) == index) // если добавляется тот же элемент в ту же позицию
                        return;                    

                    CheckDuplicatie(value); // проверка на отсутствие дубликатов
                    base[index] = value;    
                }                
            }
        }

        /// <summary>
        /// Возвращает владельца коллекции.
        /// </summary>
        public TOwner Owner
        {
            get
            {
                lock (SyncRoot)
                    return _owner;
            }
            set
            {
                lock (_syncRoot)
                {
                    _owner = value;
                }
            }
        }
    }
}
