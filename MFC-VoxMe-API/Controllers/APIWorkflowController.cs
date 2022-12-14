using AutoMapper;
using MFC_VoxMe.Core.Dtos.Common;
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
			var externalRef = movingData.GeneralInfo.EMFID;
			var jobExternalRef = movingData.GeneralInfo.Groupageid;

            var jobToCreate12 = _helpers.CreateJobObjectFromXml();
            var jsonJob = JsonConvert.SerializeObject(jobToCreate12);

			var documentPlaceHolder = _helpers.GetCorrelatingDocuments(movingData);
            var transactionToCreate = _helpers.CreateTransactionObjectFromXml();
			var jsonTransaction = JsonConvert.SerializeObject(transactionToCreate);
            var jobSummaryRequest = await _jobService.GetSummary(jobExternalRef);
			//Check if job already exist
			if (jobSummaryRequest.responseStatus != HttpStatusCode.NoContent)
			{
				var transactionSummaryRequest = await _transactionService.GetSummary(externalRef);

				//Check if transaction already exist
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
                        // Update transaction

                        //Remove and add materials to transaction
                        await _transactionService.RemoveMaterialsFromTransaction(externalRef);
						if(_helpers.GetTransactionMaterials().handedMaterials!=null)
							await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

                        //Remove and add resources to transaction
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
						// Create transaction
					    await _transactionService.CreateTransaction(transactionToCreate);

                        Random rnd = new Random();
                        byte[] b = new byte[1];
                        rnd.NextBytes(b);

						// Add documents to transaction
                        foreach (var doc in documentPlaceHolder)
                        {
                            await _transactionService.AddDocumentToTransaction(
                            new DocumentDto()
                            {
                                File = b,
                                DocTitle = doc
                            }, externalRef);
                        }

                        // Add materials to transaction
                        if (_helpers.GetTransactionMaterials().handedMaterials != null)
                            await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

                        // Add resources to transaction
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
				var jobToCreate = _helpers.CreateJobObjectFromXml();

				if (jobToCreate != null)
				{
                    // Create Job
                    await _jobService.CreateJob(jobToCreate);

					if (transactionToCreate != null)
                        //Create Transaction
						await _transactionService.CreateTransaction(transactionToCreate);

                    Random rnd = new Random();
                    byte[] b = new byte[100 * 1024];
                    rnd.NextBytes(b);

					// Add documents to transaction
                    foreach (var doc in documentPlaceHolder)
                    {
                        await _transactionService.AddDocumentToTransaction(
                        new DocumentDto()
                        {
                            File = b,
                            DocTitle = doc
                        }, externalRef);
                    }

					// Add materials to transaction
                    await _transactionService.RemoveMaterialsFromTransaction(externalRef);
                    if (_helpers.GetTransactionMaterials().handedMaterials != null)
                        await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

                    // Add resources to transaction
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
                    var jobImages = _helper.GetJobImages(jobDetailsRequest.dto);
                    var transactionImages = _helper.GetTransactionImages(transactionDetails.dto);

                    if (state == 3)
					{
                        await _helper.InsertDataFromJobDetails
                            (jobDetailsRequest.dto, movingDataId);

                        await _helper.InsertDataFromTransactionDetails
							(transactionDetails.dto, movingDataId);

                        await CreateImages(request.externalRef, jobExternalRef, filePath, jobImages,transactionImages);
                        await CreateDocuments(request.externalRef, transactionDetails.dto, filePath);

                    }
                    else
                    {
						//delete table records related to job and transaction
						await _helper.DeleteTables(movingDataId);

						// insert table recodrs related to job
						await _helper.InsertDataFromJobDetails
							(jobDetailsRequest.dto, movingDataId);
						
						// insert table recodrs related to transaction
                        await _helper.InsertDataFromTransactionDetails
							(transactionDetails.dto, movingDataId);

						//delete all images,docs in pictures, documents folder
						_helper.DeleteFilesFromFolder(new List<string>()
						{
							filePath + "Documents", 
							filePath + "Pictures"
						});

                        await CreateImages(request.externalRef, jobExternalRef, filePath, jobImages, transactionImages);
                        await CreateDocuments(request.externalRef, transactionDetails.dto, filePath);

                    }
                    //update state
                    await _helper.UpdateMovingDataStatus(status, movingDataId);
					//call webhook url to insert voxmestatus records https://edentraining.jkmoving.com:751/
					await _webhookService.PostVoxmeStatus
						(request.externalRef,movingDataId.ToString(),status.ToString());
				}
			}
            return Ok();

            async Task CreateImages(string externalRef,
                string jobRef,
                string filePath, List<string> jobImages,
                 List<string> transactionImages)
            {
                foreach (var image in jobImages)
                {
                    var response = await _transactionService.GetImageAsBinary
                                    (jobRef, "Job", image);
                    CreateImage(filePath, image, response);
                }

                foreach (var image in transactionImages)
                {
                    var response = await _transactionService.GetImageAsBinary
                                    (externalRef, "Transaction", image);
                    CreateImage(filePath, image, response);
                }

                void CreateImage(string filePath, string image, HttpResponseDto<byte[]> response)
                {
                    var bytes = response.dto;
                    string imagePath = filePath + $@"Pictures\\{image}";

                    _helper.CreateFileInFolder(imagePath, bytes);
                }
            }
            async Task CreateDocuments(string externalRef, TransactionDetailsDto transactionDetails, string filePath)
            {
                foreach (var doc in transactionDetails.documents)
                {
                    var docResponse = await _transactionService.GetDocumentAsBinary
                        (externalRef, "Transaction", doc.fileName);
                    var bytes = docResponse.dto;
                    string docPath = filePath + $@"Documents\\{doc.fileName}"; //"Documents\\test.pdf";//

                    _helper.CreateFileInFolder(docPath, bytes);
                }
            }
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
           