using SortingJobScheduler.Interfaces.Queue;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SortingJobScheduler.Queue
{
    public class SortingJobQueue : ISortingJobQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _queue = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            // Dequeue returns null if there are no items in queue, So semaphore slim used here to block the BackgroundService ExecuteAsync method execution.
            await _signal.WaitAsync(cancellationToken);
            _queue.TryDequeue(out var job);
            return job;
        }

        public void Enqueue(Func<CancellationToken, Task> job)
        {
            _ = job ?? throw new ArgumentNullException(nameof(job));

            _queue.Enqueue(job);
            _signal.Release();
        }
    }
}
