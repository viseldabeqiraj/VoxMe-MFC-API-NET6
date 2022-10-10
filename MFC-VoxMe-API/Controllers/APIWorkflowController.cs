using MFC_VoxMe_API.BusinessLogic;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Models;
using MFC_VoxMe_API.Services.Jobs;
using MFC_VoxMe_API.Services.Resources;
using MFC_VoxMe_API.Services.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Serialization;

namespace MFC_VoxMe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIWorkflowController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ITransactionService _transactionService;
        private readonly IResourceService _resourceService;

        public APIWorkflowController(IJobService jobService, ITransactionService transactionService, IResourceService resourceService)
        {
            _jobService = jobService;
            _transactionService = transactionService;
            _resourceService = resourceService;
        }
	
		//[HttpPost]
		[HttpGet]
		public async Task<ActionResult> WorkflowLogic(string xml)
        {
			try
            {
				var movingData =  Helpers.XMLParse(xml);
				var externalRef = movingData.GeneralInfo.EMFID;

				var jobDetails = await _jobService.GetDetails(externalRef);

				if (await _jobService.GetSummary(externalRef) != null)
				{
					var transactionDetails = await _transactionService.GetDetails(externalRef);
					var transactionSummary = await _transactionService.GetSummary(externalRef);

					if (transactionSummary != null)
                    {
						if (await _transactionService.GetDownloadDetails(externalRef) != null)
                        {
							//escalate to ops manager
                        }
						else
                        {
							//remove resources from transaction
							//await _transactionService.RemoveResourceFromTransaction(oldResourceCodes, externalRef);
							//assign resource to transaction
							//await _transactionService.AssignResourcesToTransaction(newResourceCodes, externalRef)
							
						}
					}
					else
                    {
						//build CreateTransactionDto from movingData obj to make creation
						//await _transactionService.CreateTransaction(externalRef);
					}
				}
				else
                {
					//Create job
					//RM details from JIM create Job in MFC
					//CreateTrasnaction
                }

				return Ok();
			}
			catch(Exception ex)
            {
				return BadRequest(ex.Message);	
            }
        }

		
	}
}
