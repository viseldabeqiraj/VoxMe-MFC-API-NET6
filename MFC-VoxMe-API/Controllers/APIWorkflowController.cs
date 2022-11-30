using AutoMapper;
using Dapper;
using MFC_VoxMe.Core.Dtos.Transactions;
using MFC_VoxMe_API.BusinessLogic.JimToVoxMe;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.Services.Jobs;
using MFC_VoxMe_API.Services.Resources;
using MFC_VoxMe_API.Services.Transactions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Linq;
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
								if (transactionDownloadDetails.responseStatus != HttpStatusCode.NoContent)
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

				if (jobToCreate != null)
				{ 
						await _jobService.CreateJob(jobToCreate);

							if (transactionToCreate != null)
								await _transactionService.CreateTransaction(transactionToCreate);

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
				return Ok();

        }

        [HttpPost("CreateResource")]
		public async Task<ActionResult> CreateResourcesLogic([FromBody] string resourceCode)
        {		
				CreateResourceDto createResourceDto = new CreateResourceDto()
				{
					resource = new CreateResourceDto.Resource()
					{
						code = resourceCode,
						resourceName = resourceCode 
					}
				};
				var createResource = await _resourceService.CreateResource(createResourceDto);

				await _resourceService.ForceConfigurationChanges("Inventory");
			return Ok();			
			
		}

        [HttpPost("DeactivateResource")]		
		public async Task<ActionResult> DeactivateResourcesLogic([FromBody]string resourceCode)
		{			
			var resourceDetails = await _resourceService.GetDetails(resourceCode);
			await _resourceService.DisableResource(resourceCode);
                    
				var resourceCodesList = _helpers.GetTransactionResources().staffResourceCodes;
				AssignStaffDesignateForemanDto resourceCodes = new AssignStaffDesignateForemanDto()
				{
					staffResourceCodes = resourceCodesList
				};
				await _resourceService.ForceConfigurationChanges("Inventory");
				return Ok();			
		}

        [HttpPost("MFCStatusUpdate")]
		public async Task<ActionResult> MFCStatusUpdate([FromBody]string externalRef, int? status, string? jobRef)
        {
			//var result = await _helpers.GetMovingDataId(externalRef);

				//var movingDataId = result.ID;
				//var jobExternalRef = result.BillOfLadingNo;

			Random rnd = new Random();
			byte[] b = new byte[100 * 1024];
			rnd.NextBytes(b);
			var doc = new DocumentDto()
			{
				File = b,
				DocTitle = "Test doc"
			};
			var x = await _transactionService.AddDocumentToTransaction(doc, "RS2222226");
			var cc = x;

			//if (jobExternalRef is not null)
			//	{
			//		var jobDetailsRequest = await _jobService.GetSummary(jobExternalRef);
			//		//TODO: Create or update correlating records
			//	}
			
			//var transactionDetails = await _transactionService.GetDetails(externalRef);
			//	var images = _helpers.GetImages(transactionDetails);
			//	foreach (var image in images)
			//	{
			//		var response = await _transactionService.GetImageAsBinary(externalRef, "Transaction", "");					
			//	}

			return Ok();
        }
	}
}
