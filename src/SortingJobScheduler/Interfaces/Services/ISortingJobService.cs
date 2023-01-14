using SortingJobScheduler.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SortingJobScheduler.Interfaces.Services
{
    /// <summary>
    /// Service to manage sorting jobs.
    /// </summary>
    public interface ISortingJobService
    {
        /// <summary>
        /// Creates a new job and returns the id
        /// </summary>
        /// <param name="data">Sequence of numbers</param>
        /// <returns>The id of the job</returns>
        string CreateJob(IEnumerable<int> data);

        /// <summary>
        /// Get a job by its id
        /// </summary>
        /// <param name="id">Job id</param>
        /// <returns></returns>
        SortingJob GetJobById(string id);

        /// <summary>
        /// Get all the jobs in the collection
        /// </summary>
        /// <returns></returns>
        IEnumerable<SortingJob> GetAllJobs();

        /// <summary>
        /// Queue a job to perform the sorting
        /// </summary>
        /// <param name="jobId">Job id</param>
        void QueueJob(string jobId);
    }
}
