using Microsoft.AspNetCore.Mvc;
using SortingJobScheduler.Interfaces.Services;
using SortingJobScheduler.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SortingJobScheduler.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SortingJobController : ControllerBase
    {
        private readonly ISortingService _sortingService;

        public SortingJobController(ISortingService sortingService)
        {
            _sortingService = sortingService ?? throw new ArgumentNullException(nameof(sortingService));
        }

        // GET: api/<SortingJobController>
        [HttpGet]
        public ActionResult<IEnumerable<SortingJob>> Get()
        {
            var jobs = _sortingService.GetAllJobs();
            return Ok(jobs);
        }

        // GET api/<SortingJobController>/5
        [HttpGet("{id}")]
        public ActionResult<SortingJob> Get(string id)
        {
            var job = _sortingService.GetJobById(id);
            return Ok(job);
        }

        // POST api/<SortingJobController>
        [HttpPost]
        public ActionResult<SortingJobCreateResponse> Post([FromBody] IEnumerable<int> numbers)
        {
            var jobId = _sortingService.CreateJob(numbers);
            return CreatedAtAction("Get", new { id = jobId }, new SortingJobCreateResponse() { Id = jobId });
        }
    }
}
