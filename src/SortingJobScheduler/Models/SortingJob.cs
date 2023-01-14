using SortingJobScheduler.Enums;
using System;
using System.Collections.Generic;

namespace SortingJobScheduler.Models
{
    public class SortingJob
    {
        public SortingJob(IEnumerable<int> data)
        {
            Data = data;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public long Duration { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Pending;

        public IEnumerable<int> Data { get; set; }
    }
}
