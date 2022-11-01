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
			try
            {
				var movingData = _helpers.XMLParse(xml);
				var externalRef = movingData.GeneralInfo.EMFID;
				var jobExternalRef = movingData.GeneralInfo.Groupageid;

				var transactionToCreate = _helpers.CreateTransactionObjectFromXml();
				var transactionToUpdate = _mapper.Map<UpdateTransactionDto>(transactionToCreate);

				var jobSummaryRequest = await _jobService.GetSummary(jobExternalRef);
				if (jobSummaryRequest != null)
				{
					if (jobSummaryRequest.responseStatus == HttpStatusCode.OK)
					{ 
						var transactionSummaryRequest = await _transactionService.GetSummary(externalRef);

						if (transactionSummaryRequest != null)
						{
							if (transactionSummaryRequest.responseStatus == HttpStatusCode.OK)
							{
								//Update transaction
								var UpdateTransactionRequest = await _transactionService.UpdateTransaction(transactionToUpdate);
								if (UpdateTransactionRequest.responseStatus != HttpStatusCode.OK)
									return BadRequest("Update Transaction Request" + UpdateTransactionRequest.responseStatus);

								if (await _transactionService.GetDownloadDetails(externalRef) != null)
								{
									//escalate to ops manager
								}
								else
								{
									//remove and assign materials to a transaction
									if (await _transactionService.RemoveMaterialsFromTransaction(externalRef))
										await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

									if (await _transactionService.RemoveResourceFromTransaction(externalRef))
									{
										var resourceCodes = _helpers.GetTransactionResources().staffResourceCodes;
										foreach (var resourceCode in resourceCodes)
										{
											AddUpdateResourcesLogic(resourceCode.code);
										}
										var AssignStaff = await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);
										if (AssignStaff.responseStatus != HttpStatusCode.OK)
											return BadRequest("AssignStaff: " + AssignStaff.responseStatus);

									}
									else
									{
										//HTTP status other than 200
										return BadRequest("RemoveResourceFromTransaction" + externalRef);
									}
								}
							}
							else
							{
								//HTTP status other than 200
								return BadRequest("TransactionSummary" + transactionSummaryRequest.responseStatus);
							}
						}
						else
                        {
							//build CreateTransactionDto from movingData obj to make creation
							if (transactionToCreate != null)
							{
								var createTransactionResponse = await _transactionService.CreateTransaction(transactionToCreate);
								if (createTransactionResponse != null)
								{
										await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);
										await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);								
								}
							}
						}						
					}
					else
					{
						//HTTP status other than 200
						return BadRequest("Job Summary: " + jobSummaryRequest.responseStatus);
					}						
				}
				else
                {
					var jobToCreate = _helpers.CreateJobObjectFromXml();
					var jobToUpdate = _mapper.Map<UpdateJobDto>(jobToCreate);

					if (jobToCreate != null)
					{ //TODO
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
				return Ok();
			}
			catch(Exception ex)
            {
				Log.Error($"Method WorkflowLogic in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
				return BadRequest(ex.Message);	
            }
        }

        [HttpPost("AddUpdateResourcesLogic")]
		public async void AddUpdateResourcesLogic(string resourceCode)
        {
			try
			{
				CreateResourceDto createResourceDto = new CreateResourceDto()
				{
					resource = new CreateResourceDto.Resource()
					{
						code = resourceCode,
						resourceName = resourceCode,
					}
				};
				await _resourceService.CreateResource(createResourceDto);


				await _resourceService.ForceConfigurationChanges("Inventory");
			}
			catch (Exception ex)
			{
				Log.Error($"Method ResourcesAddUpdateLogic in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
			}
		}

        [HttpPost("DeactivateResourcesLogic")]		
		public async void DeactivateResourcesLogic(string resourceCode)
		{
			try
			{
				var resourceDetails = await _resourceService.GetDetails(resourceCode);
				if (resourceDetails != null)
				{
					if (await _resourceService.DisableResource(resourceCode))
                    {
						var resourceCodesList = _helpers.GetTransactionResources().staffResourceCodes;
						AssignStaffDesignateForemanDto resourceCodes = new AssignStaffDesignateForemanDto()
						{
							staffResourceCodes = resourceCodesList
						};
						await _resourceService.ForceConfigurationChanges("Inventory");
					}
				}				
			}
			catch (Exception ex)
			{
				Log.Error($"Method DeactivateResourcesLogic in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
			}
		}


	}
}
