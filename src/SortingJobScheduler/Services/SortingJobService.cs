using Microsoft.Extensions.Logging;
using SortingJobScheduler.Enums;
using SortingJobScheduler.Interfaces.Queue;
using SortingJobScheduler.Interfaces.Services;
using SortingJobScheduler.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SortingJobScheduler.Services
{
    public class SortingJobService : ISortingJobService
    {
        private readonly ConcurrentDictionary<string, SortingJob> _sortingJobs;
        private readonly ISortingJobQueue _sortingJobQueue;
        private readonly ISortingService _sortingService;
        private readonly ILogger _logger;

        public SortingJobService(
            ISortingJobQueue sortingJobQueue,
            ISortingService sortingService,
            ILogger<SortingJobService> logger)
        {
            _sortingJobs = new ConcurrentDictionary<string, SortingJob>();
            _sortingJobQueue = sortingJobQueue ?? throw new ArgumentException(nameof(sortingJobQueue));
            _sortingService = sortingService ?? throw new ArgumentNullException(nameof(sortingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        public void QueueJob(string jobId)
        {
            _sortingJobQueue.Enqueue(async cancellationToken =>
            {
                await ExecuteJobAsync(jobId, cancellationToken);
            });
            _logger.LogInformation($"Sorting job with Id '{jobId}' has been added to the queue for processing");
        }

        /// <summary>
        /// Method executed by the background service to perform the sorting
        /// </summary>
        /// <param name="jobId">job id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async Task ExecuteJobAsync(string jobId, CancellationToken cancellationToken)
        {
            if (_sortingJobs.TryGetValue(jobId, out SortingJob sortingJob))
            {
                _logger.LogInformation($"Processing queued job with id '{jobId}', Time: {DateTime.Now}");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                sortingJob.Status = JobStatus.Processing;

                try
                {
                    await Task.Delay(10000, cancellationToken);
                    sortingJob.Data = _sortingService.SortNumbers(sortingJob.Data);
                    sortingJob.Status = JobStatus.Completed;
                    _logger.LogInformation($"Sorting job with id '{jobId}' completed successfully");
                }
                catch (Exception ex)
                {
                    // log error
                    sortingJob.Status = JobStatus.Failed;
                    _logger.LogError(ex, $"Sorting job with id '{jobId}' failed to execute");
                }
                finally
                {
                    stopwatch.Stop();
                    sortingJob.Duration = stopwatch.ElapsedMilliseconds;
                    _logger.LogInformation($"Sorting job with '{jobId}' completed in {sortingJob.Duration}ms");
                }
            }
            else
            {
                _logger.LogError($"Could not find a job with id '{jobId}'");
            }
        }
    }
}
