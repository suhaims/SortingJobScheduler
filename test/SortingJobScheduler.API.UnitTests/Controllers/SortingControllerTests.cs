using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using SortingJobScheduler.API.Controllers;
using SortingJobScheduler.Interfaces.Services;
using SortingJobScheduler.Models;
using SortingJobScheduler.Testing.Common.Attributes;

namespace SortingJobScheduler.API.UnitTests.Controllers
{
    public class SortingControllerTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void GetById_JobDoesNotExists_ShouldReturnNotFound(
            [Frozen] ISortingJobService sortingService,
            string jobId,
            SortingJobController sut)
        {
            // Arrange
            sortingService.GetJobById(jobId).Returns(null as SortingJob);

            // Act
            var response = sut.Get(jobId);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Theory]
        [AutoNSubstituteData]
        public void GetById_JobExists_ShouldReturnOKWithResult(
            [Frozen] ISortingJobService sortingService,
            string jobId,
            SortingJob sortingJob,
            SortingJobController sut)
        {
            // Arrange
            sortingService.GetJobById(jobId).Returns(sortingJob);

            // Act
            var response = sut.Get(jobId);

            // Assert
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(200, result?.StatusCode);
            Assert.Equal(sortingJob, result?.Value);
        }

        [Theory]
        [AutoNSubstituteData]
        public void CreateJob_EmptyInputArray_ShouldReturnValidationError(SortingJobController sut)
        {
            // Arrange
            var input = new List<int>();

            // Act
            var response = sut.Post(input);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(400, result?.StatusCode);
        }

        [Theory]
        [AutoNSubstituteData]
        public void CreateJob_ValidInputArray_ShouldCreateNewJob(
            [Frozen] ISortingJobService sortingService,
            List<int> inputArray,
            string jobId,
            SortingJobController sut)
        {
            // Arrange
            sortingService.CreateJob(inputArray).Returns(jobId);

            // Act
            var response = sut.Post(inputArray);

            // Assert
            var result = Assert.IsType<CreatedAtActionResult>(response.Result);
            Assert.Equal(201, result?.StatusCode);
            var createdResponse = Assert.IsType<SortingJobCreateResponse>(result?.Value);
            Assert.Equal(jobId, createdResponse.Id);
        }

        [Theory]
        [AutoNSubstituteData]
        public void GetAllJobs_JobsExists_ShouldReturnAllJobs(
            [Frozen] ISortingJobService sortingService,
            IEnumerable<SortingJob> sortingJobs,
            SortingJobController sut)
        {
            // Arrange
            sortingService.GetAllJobs().Returns(sortingJobs);

            // Act
            var response = sut.Get();

            // Assert
            var result = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(200, result?.StatusCode);
            var jobsResponse = Assert.IsAssignableFrom<IEnumerable<SortingJob>>(result?.Value);
            Assert.Equal(sortingJobs.Count(), jobsResponse.Count());
        }
    }
}
