using AutoMapper;
using MFC_VoxMe.Core.Dtos.Transactions;
using MFC_VoxMe.Core.Interfaces;
using MFC_VoxMe.Infrastructure.Models;
using MFC_VoxMe_API.BusinessLogic.JimToVoxMe;
using MFC_VoxMe_API.BusinessLogic.VoxMeToJim;
using MFC_VoxMe_API.Dtos.Common;
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
using static MFC_VoxMe.Infrastructure.Data.Helpers.Enums;

namespace MFC_VoxMe_API.Controllers
{
    [Route("api/[controller]")] 
	[ApiController]
    public class APIWorkflowController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ITransactionService _transactionService;
        private readonly IResourceService _resourceService;
        private readonly IVoxmeToJimHelper _helper;
        private readonly IJimToVoxmeHelper _helpers;
        private readonly IMapper _mapper;
        private readonly IWebhookService _webhookService;

        public APIWorkflowController(IJobService jobService,
			ITransactionService transactionService, 
			IResourceService resourceService, 
			IVoxmeToJimHelper helper, 
			IJimToVoxmeHelper helpers,
			IMapper mapper,
			IWebhookService webhookService)
        {
            _jobService = jobService;
            _transactionService = transactionService;
            _resourceService = resourceService;
            _helper = helper;
            _helpers = helpers;
            _mapper = mapper;
            _webhookService = webhookService;
        }


        [HttpPost("WorkflowLogic")]

		public async Task<ActionResult> WorkflowLogic([FromBody] string xml)
		{
			var movingData = _helpers.XMLParse(xml);
			//var bytes = _helpers.GetDoc();
			var externalRef = movingData.GeneralInfo.EMFID;
			var jobExternalRef = movingData.GeneralInfo.Groupageid;

            var jobToCreate12 = _helpers.CreateJobObjectFromXml();
            var jsonJob = JsonConvert.SerializeObject(jobToCreate12);

			var documentPlaceHolder = _helpers.GetCorrelatingDocuments(movingData);
            var transactionToCreate = _helpers.CreateTransactionObjectFromXml();
			var jsonTransaction = JsonConvert.SerializeObject(transactionToCreate);
			//List<ServicePaperworkModel> listDocuments = await _helper.GetPaperworkDocuments(movingData);
            var jobSummaryRequest = await _jobService.GetSummary(jobExternalRef);
			if (jobSummaryRequest.responseStatus != HttpStatusCode.NoContent)
			{
				var transactionSummaryRequest = await _transactionService.GetSummary(externalRef);

				if (transactionSummaryRequest.responseStatus != HttpStatusCode.NoContent)
				{

                    var transactionToUpdate = _mapper.Map<UpdateTransactionDto>(transactionToCreate);

                    await _transactionService.UpdateTransaction(externalRef, transactionToUpdate);


					var transactionDownloadDetails = await _transactionService.GetDownloadDetails(externalRef);
					if (transactionDownloadDetails.dto.Count > 0)
					{
						//escalate to ops manager
					}
					else
					{
						await _transactionService.RemoveMaterialsFromTransaction(externalRef);
						if(_helpers.GetTransactionMaterials().handedMaterials!=null)
							await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

						await _transactionService.RemoveResourceFromTransaction(externalRef);
						var resourceCodes = _helpers.GetTransactionResources().staffResourceCodes;
                        if (resourceCodes != null)
                        {
                            foreach (var resourceCode in resourceCodes)
                            {
                                CreateResourcesLogic(resourceCode.code).Wait();
                            }
                            await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);
                        }
					}

				}
				else
				{
					if (transactionToCreate != null)
					{
					    await _transactionService.CreateTransaction(transactionToCreate);

                        Random rnd = new Random();
                        byte[] b = new byte[1];
                        rnd.NextBytes(b);

                        foreach (var doc in documentPlaceHolder)
                        {
                            await _transactionService.AddDocumentToTransaction(
                            new DocumentDto()
                            {
                                File = b,
                                DocTitle = doc
                            }, externalRef);
                        }

                        if (_helpers.GetTransactionMaterials().handedMaterials != null)
                            await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

                        var resourceCodes = _helpers.GetTransactionResources().staffResourceCodes;
                        if (resourceCodes != null)
                        {
                            foreach (var resourceCode in resourceCodes)
                            {
                                CreateResourcesLogic(resourceCode.code).Wait();
                            }
                            await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);
                        }
                    }
				}
			}
			else
			{
                // Create Job
				var jobToCreate = _helpers.CreateJobObjectFromXml();

				if (jobToCreate != null)
				{
					await _jobService.CreateJob(jobToCreate);

					if (transactionToCreate != null)
                        //Create Transaction
						await _transactionService.CreateTransaction(transactionToCreate);

                    Random rnd = new Random();
                    byte[] b = new byte[100 * 1024];
                    rnd.NextBytes(b);

                    foreach (var doc in documentPlaceHolder)
                    {
                        await _transactionService.AddDocumentToTransaction(
                        new DocumentDto()
                        {
                            File = b,
                            DocTitle = doc
                        }, externalRef);
                    }

                    await _transactionService.RemoveMaterialsFromTransaction(externalRef);
                    if (_helpers.GetTransactionMaterials().handedMaterials != null)
                        await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

                    await _transactionService.RemoveResourceFromTransaction(externalRef);
                    var resourceCodes = _helpers.GetTransactionResources().staffResourceCodes;
                    if (resourceCodes !=null)
                    {
                        foreach (var resourceCode in resourceCodes)
                        {
                            CreateResourcesLogic(resourceCode.code).Wait();
                        }
                        await _transactionService.AssignStaffDesignateForeman(_helpers.GetTransactionResources(), externalRef);
                    }
                }
			}
			await _helpers.InsertTableRecords();
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
				var resourceDetails = await _resourceService.GetDetails(resourceCode);
				
			if (resourceDetails.responseStatus == HttpStatusCode.NotFound)
			{
                var createResource = await _resourceService.CreateResource(createResourceDto);

                await _resourceService.ForceConfigurationChanges("Inventory");
            }
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
		public async Task<ActionResult> MFCStatusUpdate(TransactionSummaryDto request)
		{
			int status = Convert.ToInt32(_helper.GetValueFromJsonConfig
				(request.onsiteStatus.Replace("Enum.TransactionOnsiteStatus.", "")));

			if (status == 21 || status == 23 || status == 24)
			{ 
				var result = await _helper.GetMovingDataId(request.externalRef);

				int movingDataId = result[0].ID;
				string jobExternalRef = result[0].BillOfLadingNo;
				int state = result[0].State;

				if (jobExternalRef is not null)
				{
					var jobDetailsRequest = await _jobService.GetDetails(jobExternalRef);
					var transactionDetails = await _transactionService.GetDetails(request.externalRef);

					string filePath = await _helper.GetItemsPath(movingDataId);

					if (state == 3)
					{
						//await _helper.InsertDataFromJobDetails
						//	(jobDetailsRequest.dto, movingDataId);

						await _helper.InsertDataFromTransactionDetails
							(transactionDetails.dto, movingDataId);

						//TODO: Create or update correlating records
						//var x = _helpers.UpdateMovingData(externalRef);

						var images = _helper.GetImages(jobDetailsRequest.dto, transactionDetails.dto);

						foreach (var image in images)
						{
							var response = await _transactionService.GetImageAsBinary
										(request.externalRef, "Transaction", image.Value);
							var bytes = response.dto;
							string imagePath = filePath + $@"Pictures\\{image.Value}"; //"Pictures\\testt.png";//

							//if (!Directory.Exists(filePath))
							//Directory.CreateDirectory(filePath);
							_helper.CreateFileInFolder(imagePath, bytes);
						}

						foreach (var doc in transactionDetails.dto.documents)
						{
							var response = await _transactionService.GetDocumentAsBinary
								(request.externalRef, "Transaction", doc.fileName);
							var bytes = response.dto;
							string docPath = filePath + $@"Documents\\{doc.fileName}"; //"Documents\\test.pdf";//

							_helper.CreateFileInFolder(docPath, bytes);
						}
					}
					else
                    {
						//delete tables for tables related to job , transaction
						await _helper.DeleteTables(movingDataId);
						await _helper.InsertDataFromJobDetails
							(jobDetailsRequest.dto, movingDataId);
						await _helper.InsertDataFromTransactionDetails
							(transactionDetails.dto, movingDataId);
						//delete all images,docs in pictures, documents folder

						_helper.DeleteFilesFromFolder(new List<string>()
						{
							filePath + "Documents", 
							filePath + "Pictures"
						});
					}
					//update state
					await _helper.UpdateMovingDataStatus(status, movingDataId);
					//call webhook url to insert voxmestatus records https://edentraining.jkmoving.com:751/
					await _webhookService.PostVoxmeStatus
						(jobExternalRef,movingDataId.ToString(),status.ToString());
				}
			}
            return Ok();
        }

        [HttpPost("SetDocument")]		
		public async Task<ActionResult> SetDocument([FromForm] string externalRef, [FromForm] IFormFile file, [FromForm] string docTitle)
        {
			byte[] fileBytes;
			if (file.Length > 0)
			{
				using (var ms = new MemoryStream())
				{
					file.CopyTo(ms);
					fileBytes = ms.ToArray();
				}
				var result = await _transactionService.AddDocumentToTransaction(
				new DocumentDto()
				{
					DocTitle = docTitle,
					File = fileBytes
				}, externalRef);
			}
				

			return Ok();
        }
	}
}
           