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
		
		//[HttpPost("WorkflowLogic")]
		[HttpGet]
		public async Task<ActionResult> WorkflowLogic(string xml)
        {
			try
            {
				var movingData = _helpers.XMLParse(xml);
				var externalRef = movingData.GeneralInfo.EMFID;
				var jobExternalRef = movingData.GeneralInfo.Groupageid;

				var transactionToCreate = _helpers.CreateTransactionObjectFromXml();
				var transactionToUpdate = _mapper.Map<UpdateTransactionDto>(transactionToCreate);

				var jobSummary = await _jobService.GetSummary(jobExternalRef);
				if (jobSummary != null)
				{
					if (jobSummary.responseStatus == HttpStatusCode.OK)
					{ 
						var transactionSummary = await _transactionService.GetSummary(externalRef);

						if (transactionSummary != null)
						{
							if (transactionSummary.responseStatus == HttpStatusCode.OK)
							{
								//Update transaction
								await _transactionService.UpdateTransaction(transactionToUpdate);
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
										await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);
									}
								}
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
						return BadRequest(jobSummary.responseStatus);
					}						
				}
				else
                {
					var jobToCreate = _helpers.CreateJobObjectFromXml();
					var jobToUpdate = _mapper.Map<UpdateJobDto>(jobToCreate);

					if (jobToCreate != null)
					await _jobService.CreateJob(jobToCreate);

					if (transactionToCreate != null)
						await _transactionService.CreateTransaction(transactionToCreate);
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
