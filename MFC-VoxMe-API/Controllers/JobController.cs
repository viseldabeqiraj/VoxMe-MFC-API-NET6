using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Services.Jobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MFC_VoxMe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }
        [HttpGet("JobDetails")]
        public async Task<ActionResult> GetDetails(string externalRef)
        {
            try
            {
                return Ok(await _jobService.GetDetails(externalRef));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("JobSummary")]
        public async Task<ActionResult> GetSummary(string externalRef)
        {
            try
            {
                return Ok(await _jobService.GetSummary(externalRef));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateJob")]
        public async Task<ActionResult> CreateJob(CreateJobDto request)
        {
            try
            {
                return Ok(await _jobService.CreateJob(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
