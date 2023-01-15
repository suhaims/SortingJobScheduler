using Microsoft.AspNetCore.Mvc;
using SortingJobScheduler.Interfaces.Services;
using SortingJobScheduler.Models;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SortingJobScheduler.API.Controllers
{
    /// <summary>
    /// Sorting Job API which allows callers to manage sorting jobs
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SortingJobController : ControllerBase
    {
        private readonly ISortingJobService _sortingService;
        private readonly ILogger _logger;

        public SortingJobController(
            ISortingJobService sortingService, 
            ILogger<SortingJobController> logger)
        {
            _sortingService = sortingService ?? throw new ArgumentNullException(nameof(sortingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves a lost of sorting jobs
        /// </summary>
        /// <returns>An array of sorting jobs</returns>
        /// <response code="200">Sorting jobs retrieved successfully</response>
        [HttpGet]
        public ActionResult<IEnumerable<SortingJob>> Get()
        {
            _logger.LogInformation($"GET All Jobs API call received at {DateTime.Now}");
            var jobs = _sortingService.GetAllJobs();
            _logger.LogInformation($"Found {jobs.Count()} sorting jobs");

            return Ok(jobs);
        }

        /// <summary>
        /// Gets a job by id
        /// </summary>
        /// <param name="id">The sorting job id</param>
        /// <returns>A sorting job found by the job id</returns>
        /// <response code="200">Returns the sorting job found by the job id.</response>
        /// <response code="404">Invalid job id.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SortingJob))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SortingJob> Get([Required]string id)
        {
            _logger.LogInformation($"GET Job By Id API call received at {DateTime.Now}, Id: {id}");
            var job = _sortingService.GetJobById(id);
            if (job == null)
            {
                _logger.LogInformation($"Could not find a job with id '{id}'");
                return NotFound();
            }

            return Ok(job);
        }

        /// <summary>
        /// Create a new sorting job
        /// </summary>
        /// <param name="numbers">Unsorted array of numbers</param>
        /// <returns>A newly created sorting job</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/sortingjob
        ///     [10,5,11,1,67,23,5,2]
        ///
        /// </remarks>
        /// <response code="201">Sorting job created successfully.</response>
        /// <response code="400">Validation error.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SortingJobCreateResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponseHeader(StatusCodes.Status201Created, "location", "string", "Location of the newly created Sorting Job")]
        public ActionResult<SortingJobCreateResponse> Post([FromBody] IEnumerable<int> numbers)
        {
            _logger.LogInformation($"POST Create Job API call received at {DateTime.Now}");

            if (!numbers.Any())
            {
                return ValidationProblem(new ValidationProblemDetails
                {
                    Detail = "Array of numbers should not be empty",
                    Status = 400
                });
            }

            var jobId = _sortingService.CreateJob(numbers);
            _sortingService.QueueJob(jobId);

            return CreatedAtAction("Get", new { id = jobId }, new SortingJobCreateResponse() { Id = jobId });
        }
    }
}
