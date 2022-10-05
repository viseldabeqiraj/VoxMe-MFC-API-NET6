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
                var result = await _jobService.GetDetails(externalRef);

                if (result != null)
                    return Ok(result);
                else return BadRequest();
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
                var result = await _jobService.GetSummary(externalRef);

                if (result != null)
                    return Ok(result);
                else return BadRequest();

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
                var result = await _jobService.CreateJob(request);

                if (result != null)
                    return Ok(result);
                else return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateJob")]
        public async Task<ActionResult> UpdateJob(UpdateJobDto request)
        {
            try
            {
                var result = await _jobService.UpdateJob(request);

                if (result != null)
                    return Ok(result);
                else return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
