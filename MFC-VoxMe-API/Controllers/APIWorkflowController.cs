using AutoMapper;
using MFC_VoxMe_API.BusinessLogic;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.Models;
using MFC_VoxMe_API.Services.Jobs;
using MFC_VoxMe_API.Services.Resources;
using MFC_VoxMe_API.Services.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
	
		//[HttpPost]
		[HttpGet]
		public async Task<ActionResult> WorkflowLogic(string xml)
        {
			try
            {
				var movingData = _helpers.XMLParse(xml);
				var externalRef = movingData.GeneralInfo.EMFID;
				var jobExternalRef = movingData.GeneralInfo.Groupageid;
				//await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

				var jobToCreate = _helpers.CreateJobObjectFromXml();
				//var transactionToCreate = Helpers.CreateTransactionObjectFromXml();
				//TODO: Other fields to add
				var transactionToCreate = _mapper.Map<CreateTransactionDto>(jobToCreate);

				var jobToUpdate = _mapper.Map<UpdateJobDto>(jobToCreate);

				var jobDetails = await _jobService.GetDetails(jobExternalRef);

				if (await _jobService.GetSummary(jobExternalRef) != null)
				{
					var transactionDetails = await _transactionService.GetDetails(externalRef);
					var transactionSummary = await _transactionService.GetSummary(externalRef);

					if (transactionSummary != null)
                    {
						//Update transaction
						//await _transactionService.UpdateTransaction();
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
								var resourceCodes = _helpers.GetTransactionResources().resourceCodes;
								foreach (var resourceCode in resourceCodes)
                                {
									ResourcesAddUpdateLogic(resourceCode);
								}
								await _transactionService.AssignResourcesToTransaction(_helpers.GetTransactionResources(), externalRef);
							}
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
					{
						await _transactionService.CreateTransaction(transactionToCreate);
						await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);
						await _transactionService.AssignResourcesToTransaction(_helpers.GetTransactionResources(), externalRef);
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

		public async void ResourcesAddUpdateLogic(string resourceCode)
        {
			try
            {
				var resourceDetails = await _resourceService.GetDetails(resourceCode);
				if (resourceDetails != null)
                {
					UpdateResourceDto updateResourceDto = new UpdateResourceDto()
					{
						resourceName = resourceCode
					};
					await _resourceService.UpdateResource(updateResourceDto, resourceCode); //?? gets updated with the same name because code already exists
                }
				else
                {
					CreateResourceDto createResourceDto = new CreateResourceDto()
					{
						resource = new CreateResourceDto.Resource()
                        {
							code = resourceCode,
							resourceName = resourceCode,
							resourceType = "packer",
                        }
					};
					await _resourceService.CreateResource(createResourceDto);
				}
				var resourceCodesList = _helpers.GetTransactionResources().resourceCodes;
				ResourceCodesForTransactionDto resourceCodes = new ResourceCodesForTransactionDto()
				{
					resourceCodes = resourceCodesList
				};
				await _resourceService.ForceConfigurationChanges(resourceCodes, "");
            }
            catch(Exception ex)
            {
				Log.Error($"Method ResourcesAddUpdateLogic in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
			}
		}

		
	}
}
