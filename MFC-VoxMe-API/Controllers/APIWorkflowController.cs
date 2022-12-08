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
			    var movingData = await _helpers.XMLParseAsync(xml);
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
											await CreateResourcesLogic(resourceCode.code);
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

					//await _transactionService.RemoveMaterialsFromTransaction(externalRef);
					//await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

					//await _transactionService.RemoveResourceFromTransaction(externalRef);

					//var resourceCodes = _helpers.GetTransactionResources().staffResourceCodes;
					//foreach (var resourceCode in resourceCodes)
					//{
					//	await CreateResourcesLogic(resourceCode.code);
					//}
					//await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);
					await _helpers.InsertTableRecords();
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
		public async Task<ActionResult> MFCStatusUpdate([FromBody] string externalRef, string status, string? jobRef)
		{
			status = status.Replace("Enum.TransactionOnsiteStatus.", "");
			if (status == 21 || status == 23 || status == 24)
			{ 
            var result = await _helpers.GetMovingDataId(externalRef);

            int movingDataId = result[0].ID;
            string jobExternalRef = result[0].BillOfLadingNo;
            if (jobExternalRef is not null)
            {
                var jobDetailsRequest = await _jobService.GetSummary(jobExternalRef);
				//TODO: Create or update correlating records
				//var x = _helpers.UpdateMovingData(externalRef);

			}

			var transactionDetails = await _transactionService.GetDetails(externalRef);
            var images = _helpers.GetImages(transactionDetails);
				foreach (var image in images)
				{
					var response = await _transactionService.GetImageAsBinary(externalRef, "Transaction", image.Value);
					var bytes = response.dto;
					//select itemspath from prefs for that movingid
					string filePath = await _helpers.GetItemsPath(movingDataId) + "\\test3.png"; //image.Value

					if (!Directory.Exists(filePath))
					{
						//Directory.CreateDirectory(filePath);
						using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
						{
							stream.Write(bytes);
						}
					}
				}
			}

            return Ok();
        }
	}
}
