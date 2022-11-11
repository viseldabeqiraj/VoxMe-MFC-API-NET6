using AutoMapper;
using MFC_VoxMe_API.BusinessLogic;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.Services.Jobs;
using MFC_VoxMe_API.Services.Resources;
using MFC_VoxMe_API.Services.Transactions;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace MFC_VoxMe_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIWorkflowController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ITransactionService _transactionService;
        private readonly IResourceService _resourceService;
        private readonly IHelpers _helpers;
        private readonly IMapper _mapper;

		public APIWorkflowController(IJobService jobService, ITransactionService transactionService, IResourceService resourceService, IHelpers helpers,IMapper mapper)
        {
            _jobService = jobService;
            _transactionService = transactionService;
            _resourceService = resourceService;
            _helpers = helpers;
            _mapper = mapper;
        }
		
		[HttpPost("WorkflowLogic")]

		public async Task<ActionResult> WorkflowLogic([FromBody] string xml)
        {
			    var movingData = _helpers.XMLParse(xml);
				var externalRef = movingData.GeneralInfo.EMFID;
				var jobExternalRef = movingData.GeneralInfo.Groupageid;

				var transactionToCreate = _helpers.CreateTransactionObjectFromXml();
				var transactionToUpdate = _mapper.Map<UpdateTransactionDto>(transactionToCreate);

				var jobSummaryRequest = await _jobService.GetSummary(jobExternalRef);
				if (jobSummaryRequest.responseStatus != HttpStatusCode.NoContent)
				{
						var transactionSummaryRequest = await _transactionService.GetSummary(externalRef);

						if (transactionSummaryRequest.responseStatus != HttpStatusCode.NoContent)
						{
								 await _transactionService.UpdateTransaction(externalRef,transactionToUpdate);
								
								
								var transactionDownloadDetails = await _transactionService.GetDownloadDetails(externalRef);
								if (transactionDownloadDetails.responseStatus == HttpStatusCode.OK)
								{
									//escalate to ops manager
								}
								else
								{
									await _transactionService.RemoveMaterialsFromTransaction(externalRef);
									await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

									await _transactionService.RemoveResourceFromTransaction(externalRef);
									
										var resourceCodes = _helpers.GetTransactionResources().staffResourceCodes;
										foreach (var resourceCode in resourceCodes)
										{
											CreateResourcesLogic(resourceCode.code);
										}
										await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);										
								}
							
						}
						else
                        {
							if (transactionToCreate != null)
							{
								var createTransactionRequest = await _transactionService.CreateTransaction(transactionToCreate);
								await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);
								await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);																
							}
						}						
				}
				else
                {
					var jobToCreate = _helpers.CreateJobObjectFromXml();
					var jobToUpdate = _mapper.Map<UpdateJobDto>(jobToCreate);

					if (jobToCreate != null)
					{ 
						await _jobService.CreateJob(jobToCreate);

							if (transactionToCreate != null)
								await _transactionService.CreateTransaction(transactionToCreate);					
					}
				}
				return Ok();

        }

        [HttpPost("CreateResource")]
		public async Task<ActionResult> CreateResourcesLogic([FromBody] string resourceCode)
        {
			try
			{
				CreateResourceDto createResourceDto = new CreateResourceDto()
				{
					resource = new CreateResourceDto.Resource()
					{
						code = resourceCode,
						resourceName = resourceCode //split TODO,
					}
				};
				var createResource = await _resourceService.CreateResource(createResourceDto);
				if (createResource.responseStatus == HttpStatusCode.OK)
                {
					if (await _resourceService.ForceConfigurationChanges("Inventory"))
						return Ok();
				}
				return BadRequest("CreateResource Request: " + createResource.responseStatus);
			}
			catch (Exception ex)
			{
				Log.Error($"Method ResourcesAddUpdateLogic in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
				return BadRequest(ex.Message);
			}
		}

        [HttpPost("DeactivateResource")]		
		public async Task<ActionResult> DeactivateResourcesLogic([FromBody]string resourceCode)
		{
			try
			{
				var resourceDetails = await _resourceService.GetDetails(resourceCode);
				if (resourceDetails.responseStatus == HttpStatusCode.OK)
				{
					if (await _resourceService.DisableResource(resourceCode))
                    {
						var resourceCodesList = _helpers.GetTransactionResources().staffResourceCodes;
						AssignStaffDesignateForemanDto resourceCodes = new AssignStaffDesignateForemanDto()
						{
							staffResourceCodes = resourceCodesList
						};
						if (await _resourceService.ForceConfigurationChanges("Inventory"))
						return Ok();
					}
				}
				return BadRequest("ResourceDetails" + resourceDetails.responseStatus);
			}
			catch (Exception ex)
			{
				Log.Error($"Method DeactivateResourcesLogic in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
				return BadRequest(ex.Message);
			}
		}


	}
}
