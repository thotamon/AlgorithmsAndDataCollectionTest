using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ParallelQueque
{
    public class ParallelQueue<T>
    {
        private Queue<T> _queue = new Queue<T>();
        private readonly object _locker = new object();
        private volatile int _count = 0;

        public T Pop()
        {
            while (true)
            {
                SpinWait.SpinUntil(() => !IsEmpty());
                lock (this._locker)
                {
                    if (this._queue.Any())
                    {
                        var item = this._queue.Dequeue();
                        this._count--;
                        return item;
                    }
                }
            }
        }

        public void Push(T item)
        {
            lock (this._locker)
            {
                this._queue.Enqueue(item);
                this._count++;
            }
        }

        public bool IsEmpty() => this._count == 0;
    }
}
