using System.Threading.Tasks;
using System.Threading;
using System;

namespace SortingJobScheduler.Interfaces.Queue
{
    public interface ISortingJobQueue
    {
        void Enqueue(Func<CancellationToken, Task> job);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
