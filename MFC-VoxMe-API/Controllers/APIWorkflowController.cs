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
			//try
   //         {
			    var movingData = _helpers.XMLParse(xml);
				var externalRef = movingData.GeneralInfo.EMFID;
				var jobExternalRef = movingData.GeneralInfo.Groupageid;

				var transactionToCreate = _helpers.CreateTransactionObjectFromXml();
				var transactionToUpdate = _mapper.Map<UpdateTransactionDto>(transactionToCreate);

				var jobSummaryRequest = await _jobService.GetSummary(jobExternalRef);
				if (jobSummaryRequest.responseStatus != HttpStatusCode.NoContent)
				{
					if (jobSummaryRequest.responseStatus == HttpStatusCode.OK)
					{ 
						var transactionSummaryRequest = await _transactionService.GetSummary(externalRef);

						if (transactionSummaryRequest.responseStatus != HttpStatusCode.NoContent)
						{
							if (transactionSummaryRequest.responseStatus == HttpStatusCode.OK)
							{
								var UpdateTransactionRequest = await _transactionService.UpdateTransaction(transactionToUpdate);
								if (UpdateTransactionRequest.responseStatus != HttpStatusCode.OK)
									return BadRequest("Update Transaction Request" + UpdateTransactionRequest.responseStatus);
								
								var transactionDownloadDetails = await _transactionService.GetDownloadDetails(externalRef);
								if (transactionDownloadDetails.responseStatus == HttpStatusCode.OK)
								{
									//escalate to ops manager
								}
								else
								{
									if (await _transactionService.RemoveMaterialsFromTransaction(externalRef))
										await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);
									else return BadRequest("RemoveMaterialsFromTransaction" + externalRef);

									if (await _transactionService.RemoveResourceFromTransaction(externalRef))
									{
										var resourceCodes = _helpers.GetTransactionResources().staffResourceCodes;
										foreach (var resourceCode in resourceCodes)
										{
											CreateResourcesLogic(resourceCode.code);
										}
										var AssignStaff = await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);
										if (AssignStaff.responseStatus != HttpStatusCode.OK)
											return BadRequest("AssignStaff: " + AssignStaff.responseStatus);
									}
									else	//HTTP status other than 200
										return BadRequest("RemoveResourceFromTransaction" + externalRef);
								}
							}
							else							
								//HTTP status other than 200
								return BadRequest("TransactionSummary" + transactionSummaryRequest.responseStatus);
							
						}
						else
                        {
							if (transactionToCreate != null)
							{
								var createTransactionRequest = await _transactionService.CreateTransaction(transactionToCreate);
								if (createTransactionRequest != null)
								{
										await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);
										await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);								
								}
							}
						}						
					}
					else					
						//HTTP status other than 200
						return BadRequest("Job Summary: " + jobSummaryRequest.responseStatus);										
				}
				else
                {
					var jobToCreate = _helpers.CreateJobObjectFromXml();
					var jobToUpdate = _mapper.Map<UpdateJobDto>(jobToCreate);

					if (jobToCreate != null)
					{ 
						var CreateJobRequest = await _jobService.CreateJob(jobToCreate);
						if (CreateJobRequest.responseStatus == HttpStatusCode.OK)
						{

							if (transactionToCreate != null)
							{
								var createTransactionRequest = await _transactionService.CreateTransaction(transactionToCreate);
								if (createTransactionRequest.responseStatus != HttpStatusCode.OK)
									return BadRequest("createTransactionAPI: " + createTransactionRequest.responseStatus);
							}
						}
						else return BadRequest("CreateJobRequest:" + CreateJobRequest.responseStatus);
					}
				}
				return BadRequest("testttttt");
			//}
			//catch(Exception ex)
   //         {
			//	Log.Error($"Method WorkflowLogic in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
			//	return BadRequest(ex.Message);	
   //         }
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
