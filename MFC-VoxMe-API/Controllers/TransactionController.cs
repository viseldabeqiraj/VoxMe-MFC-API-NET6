using MFC_VoxMe_API.Services.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MFC_VoxMe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("JobDetails")]
        public async Task<ActionResult> GetDetails(string externalRef)
        {
            try
            {
                var result = await _transactionService.GetDetails(externalRef);

                if (result != null)
                    return Ok(result);
                else return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("JobSummary")]
        public async Task<ActionResult> GetSummary(string externalRef)
        {
            try
            {
                var result = await _transactionService.GetSummary(externalRef);

                if (result != null)
                    return Ok(result);
                else return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpPost("uploadFile")]
        //public async Task<ActionResult> AddDocumentToTransaction(IFormFile File, string DocTitle, string externalRef)
        //{
        //    try
        //    {
        //        var result = await _transactionService.AddDocumentToTransaction(File, DocTitle, externalRef);

                if (result != null)
                    return Ok(result);
                else return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("CreateJob")]
        //public async Task<ActionResult> CreateJob(CreateJobDto request)
        //{
        //    try
        //    {
        //        var result = await _jobService.CreateJob(request);

        //        if (result != null)
        //            return Ok(result);
        //        else return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPut("UpdateJob")]
        //public async Task<ActionResult> UpdateJob(UpdateJobDto request)
        //{
        //    try
        //    {
        //        var result = await _jobService.UpdateJob(request);

        //        if (result != null)
        //            return Ok(result);
        //        else return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

    }
}
