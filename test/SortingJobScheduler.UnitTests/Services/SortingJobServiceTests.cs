using SortingJobScheduler.Enums;
using SortingJobScheduler.Services;
using SortingJobScheduler.Testing.Common.Attributes;

namespace SortingJobScheduler.UnitTests.Services
{
    public class SortingJobServiceTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void CreateJob_JobCreated_ShouldBeAbleToRetrieveById(SortingJobService sut)
        {
            // Arrange
            var unsortedArray = new List<int> { 10, 5, 9, 22 };

            // Act
            var jobId = sut.CreateJob(unsortedArray);
            var job = sut.GetJobById(jobId);

            // Asset
            Assert.Equal(jobId, job.Id);
            Assert.Equal(unsortedArray, job.Data);
        }

        [Theory]
        [AutoNSubstituteData]
        public void CreateJob_JobCreated_InitialStatusShouldBePending(SortingJobService sut)
        {
            // Arrange
            var unsortedArray = new List<int> { 10, 5, 9, 22 };

            // Act
            var jobId = sut.CreateJob(unsortedArray);
            var job = sut.GetJobById(jobId);

            // Asset
            Assert.Equal(JobStatus.Pending, job.Status);
            Assert.Equal(0, job.Duration);
        }

        [Theory]
        [AutoNSubstituteData]
        public void CreateMultipleJobs_JobsCreated_ShouldBeAbleToRetrieveById(SortingJobService sut)
        {
            // Arrange
            var unsortedArray = new List<int> { 10, 5, 9, 22 };

            // Act
            var jobAId = sut.CreateJob(unsortedArray);
            var jobBId = sut.CreateJob(unsortedArray);
            
            var jobB = sut.GetJobById(jobBId);

            // Asset
            Assert.Equal(jobBId, jobB.Id);
            Assert.Equal(unsortedArray, jobB.Data);
        }

        [Theory]
        [AutoNSubstituteData]
        public void CreateMultipleJobs_JobsCreated_ShouldBeAbleToRetrieveAllJobs(SortingJobService sut)
        {
            // Arrange
            var unsortedArray = new List<int> { 10, 5, 9, 22 };

            // Act
            sut.CreateJob(unsortedArray);
            sut.CreateJob(unsortedArray);

            var jobs = sut.GetAllJobs();

            // Asset
            Assert.Equal(2, jobs.Count());
        }
    }
}
