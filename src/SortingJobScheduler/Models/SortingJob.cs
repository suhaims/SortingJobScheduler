using SortingJobScheduler.Enums;
using System;
using System.Collections.Generic;

namespace SortingJobScheduler.Models
{
    /// <summary>
    /// A job for sorting list of numbers
    /// </summary>
    public class SortingJob
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortingJob"/>
        /// </summary>
        /// <param name="data">Input data that needs to be sorted</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SortingJob(IEnumerable<int> data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// The unique Id of the job
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The datetime the job was created
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// Time taken to execute the job in millisecond
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// Current status of the job
        /// </summary>
        public JobStatus Status { get; set; } = JobStatus.Pending;

        /// <summary>
        /// Array of numbers
        /// </summary>
        public IEnumerable<int> Data { get; set; }
    }
}
