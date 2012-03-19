using System;
using System.Collections;
using System.Collections.Generic;

namespace Easis.Common.Collections
{
    public class SyncronizedQueue<T> : IEnumerable<T>, ICollection
    {
        private readonly Queue<T> _queue;
        private readonly object _syncRoot = new object();

        public SyncronizedQueue()
        {
            _queue = new Queue<T>();
        }

        public SyncronizedQueue(int capacity)
        {
            _queue = new Queue<T>(capacity);
        }

        public SyncronizedQueue(IEnumerable<T> collection)
        {
            _queue = new Queue<T>(collection);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Queue<T> localQ;
            // init enumerator
            lock (_syncRoot)
            {
                // create a copy of m_TList
                localQ = new Queue<T>(_queue);
            }
            // get the enumerator
            return ((IEnumerable<T>) localQ).GetEnumerator(); 
        }

        /// <summary>
        /// Adds an item to the queue
        /// </summary>
        /// <param name="item">the item to add to the queue</param>
        public virtual void Enqueue(T item)
        {
            lock (_syncRoot)
                _queue.Enqueue(item);
        }

        /// <summary>
        /// Removes and returns the item in the beginning of the queue
        /// Syncronous method. If there is no elements, it blockes execution.
        /// </summary>
        public virtual T Dequeue()
        {
            lock (_syncRoot)
                return _queue.Dequeue();
        }

        public virtual T Peek()
        {
            lock (_syncRoot)
            {
                return _queue.Peek();
            }
        }

        /// <summary>
        /// Enqueues the list of items
        /// </summary>
        /// <param name="itemsToQueue">list of items to enqueue</param>
        public virtual void EnqueueAll(IEnumerable<T> itemsToQueue)
        {
            lock (_syncRoot)
            {
                // loop through and add each item
                foreach (T item in itemsToQueue)
                    _queue.Enqueue(item);
            }
        }

        public void CopyTo(Array array, int index)
        {
            lock (_syncRoot)
            {
                T [] at = new T[_queue.Count];
                _queue.CopyTo(at, 0);
                Array.Copy(at, 0, array, index, at.Length);
            }
        }

        public int Count
        {
            get {
                lock (_syncRoot)
                    return _queue.Count;
            }
        }

        // TODO: зачем это отдавать наружу?
        public object SyncRoot
        {
            get { return _syncRoot; }
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public override string ToString()
        {
            string ret = "[ ";
            lock (_syncRoot)
            {
                foreach (T vv in _queue)
                {
                    ret += vv + ", ";
                }
            }
            ret += "]";
            return ret;
        }
    }
}