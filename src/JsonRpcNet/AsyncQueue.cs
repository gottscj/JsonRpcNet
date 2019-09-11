using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace JsonRpcNet
{
    public class AsyncQueue<T>
    {
        private readonly SemaphoreSlim _sem;
        private readonly ConcurrentQueue<T> _que;

        public AsyncQueue()
        {
            _sem = new SemaphoreSlim(0);
            _que = new ConcurrentQueue<T>();
        }

        public void Enqueue(T item)
        {
            _que.Enqueue(item);
            _sem.Release();
        }

        public async Task<T> DequeueAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            for (; ; )
            {
                await _sem.WaitAsync(cancellationToken);

                if (_que.TryDequeue(out var item))
                {
                    return item;
                }
            }
        }
    }
}