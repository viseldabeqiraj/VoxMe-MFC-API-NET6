using AutoMapper;
using MFC_VoxMe_API.BusinessLogic;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
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
        private readonly IMapper _mapper;

        public APIWorkflowController(IJobService jobService, ITransactionService transactionService, IResourceService resourceService, IMapper mapper)
        {
            _jobService = jobService;
            _transactionService = transactionService;
            _resourceService = resourceService;
            _mapper = mapper;
        }
	
		//[HttpPost]
		[HttpGet]
		public async Task<ActionResult> WorkflowLogic(string xml)
        {
			try
            {
				var movingData =  Helpers.XMLParse(xml);
				var externalRef = movingData.GeneralInfo.EMFID;

				var jobToCreate = Helpers.CreateJobObjectFromXml();
				var transactionToCreate = Helpers.CreateTransactionObjectFromXml();
			
				var jobToUpdate = _mapper.Map<UpdateJobDto>(jobToCreate);

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

						if (transactionToCreate != null)
							await _transactionService.CreateTransaction(transactionToCreate);
					}
				}
				else
                {
					//Create job
					

					if (jobToCreate != null)
					await _jobService.CreateJob(jobToCreate);
					//RM details from JIM create Job in MFC
					//CreateTrasnaction

					if (transactionToCreate != null)
						await _transactionService.CreateTransaction(transactionToCreate);
					//Transaction/AssignResources ->add crew, trucks, materials
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
