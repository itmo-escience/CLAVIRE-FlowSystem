using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Easis.Wfs.Interpreting.Utils
{
    // TODO: use Easis.Common.Collections
    public class SyncronizedQueue<T> : IEnumerable<T>, ICollection
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly object _syncRoot = new object();

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
            foreach (T item in localQ)
                yield return item; 
        }

        /// <summary>
        /// Adds an item to the queue
        /// </summary>
        /// <param name="item">the item to add to the queue</param>
        public virtual void Enqueue(T item)
        {
            lock (_syncRoot)
            {
                _queue.Enqueue(item);
            }
        }

        /// <summary>
        /// Removes and returns the item in the beginning of the queue
        /// Syncronous method. If there is no elements, it blockes execution.
        /// </summary>
        public virtual T Dequeue()
        {
            lock (_syncRoot)
            {
                return _queue.Dequeue();
            }
        }

        /// <summary>
        /// Enqueues the list of items
        /// </summary>
        /// <param name="ItemsToQueue">list of items to enqueue</param>
        public virtual void EnqueueAll(IList<T> ItemsToQueue)
        {
            lock (_syncRoot)
            {
                // loop through and add each item
                foreach (T item in ItemsToQueue)
                    _queue.Enqueue(item);
            }
        }

        public void CopyTo(Array array, int index)
        {
            lock (_syncRoot)
            {
                T[] at = new T[_queue.Count];
                _queue.CopyTo(at,0);
                System.Array.Copy(at,0,array,index,at.Length);
            }
        }

        public int Count
        {
            get {
                lock (_syncRoot)
                    return _queue.Count;
            }
        }

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
            //lock (_syncRoot)
            //{
            //    foreach (T vv in _queue)
            //    {
            //        ret += vv + ", ";
            //    }
            //}
            ret += "]";
            return ret;
        }
    }
    public class SyncronizedBlockingQueue<T> : SyncronizedQueue<T>
    {
        private readonly SemaphoreSlim _sem = new SemaphoreSlim(0);

        public override void Enqueue(T item)
        {
            base.Enqueue(item);
            _sem.Release(1);
        }

        public override T Dequeue()
        {
            _sem.Wait();
            return base.Dequeue();
        }

        /// <summary>
        /// throws TimeoutException
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public T Dequeue(TimeSpan timeout)
        {
            if (_sem.Wait(timeout))
            {
                return base.Dequeue();
            }
            else
            {
                throw new TimeoutException();
            }
        }

        public override void EnqueueAll(IList<T> ItemsToQueue)
        {
            base.EnqueueAll(ItemsToQueue);
            _sem.Release(ItemsToQueue.Count);
        }
    }
}