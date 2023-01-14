using SortingJobScheduler.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SortingJobScheduler.Interfaces.Services
{
    public interface ISortingService
    {
        string CreateJob(IEnumerable<int> data);

        SortingJob GetJobById(string id);

        IEnumerable<SortingJob> GetAllJobs();
        
        Task QueueJobAsync(string jobId);
    }
}
