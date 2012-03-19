using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Easis.Common.Collections
{

#if SILVERLIGHT
    public class SemaphoreSlim
    {
        private readonly object _locker = new object();
        private int _count = 0;
        private int _max = 0;

        public SemaphoreSlim(int initialCount)
        {
            _count = initialCount;
        }

        public SemaphoreSlim(int initialCount, int max)
        {
            _count = initialCount;
            _max = max;
        }

        public int CurrentCount
        {
            get
            {
                lock (_locker) return _count;
            }
        }

        public void Wait()
        {
            while (true)
            {
                lock (_locker)
                    if (_count < _max || _max == 0)
                    {
                        _count++;
                        return;
                    }

                Thread.Sleep(50);
            }
        }

        public void Release()
        {
            Release(1);
        }

        public void Release(int releaseNum)
        {
            if (releaseNum < 1)
                throw new ArgumentOutOfRangeException("releaseNum");

            lock (_locker)
                if (_count >= releaseNum - 1)
                    _count -= releaseNum;
        }
    }
#endif

    public class SyncronizedBlockingQueue<T> : SyncronizedQueue<T>
    {
        private readonly SemaphoreSlim _sem = new SemaphoreSlim(0);

        public SyncronizedBlockingQueue()
        {
        }

        public SyncronizedBlockingQueue(int capacity): base(capacity)
        {
        }

        public SyncronizedBlockingQueue(IEnumerable<T> collection): base(collection)
        {
        }

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

        public override void EnqueueAll(IEnumerable<T> itemsToQueue)
        {
            base.EnqueueAll(itemsToQueue);
            _sem.Release(itemsToQueue.Count());
        }
    }
}