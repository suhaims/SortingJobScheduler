using SortingJobScheduler.Interfaces.Services;
using SortingJobScheduler.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SortingJobScheduler.Services
{
    public class SortingService: ISortingService
    {
        private readonly ConcurrentDictionary<string, SortingJob> _sortingJobs;

        public SortingService()
        {
            _sortingJobs = new ConcurrentDictionary<string, SortingJob>();
        }

        public string CreateJob(IEnumerable<int> data)
        {
            _ = data ?? throw new ArgumentNullException(nameof(data));

            var job = new SortingJob(data);
            _sortingJobs.TryAdd(job.Id, job);

            return job.Id;
        }

        public SortingJob GetJobById(string id)
        {
            _sortingJobs.TryGetValue(id, out SortingJob job);
            return job;
        }

        public IEnumerable<SortingJob> GetAllJobs()
        {
            return _sortingJobs.Values.OrderByDescending(j => j.Timestamp);
        }

        public Task QueueJobAsync(string jobId)
        {
            // TODO: Process queued job
            return Task.CompletedTask;
        }
    }
}
