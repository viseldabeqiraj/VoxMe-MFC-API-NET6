using AutoMapper;
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
			var bytes = _helpers.GetDoc();
			var externalRef = movingData.GeneralInfo.EMFID;
			var jobExternalRef = movingData.GeneralInfo.Groupageid;

            var jobToCreate12 = _helpers.CreateJobObjectFromXml();
            var jsonJob = JsonConvert.SerializeObject(jobToCreate12);

			var transactionToCreate = _helpers.CreateTransactionObjectFromXml();
			var jsonTransaction = JsonConvert.SerializeObject(transactionToCreate);

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
						await _transactionService.AssignMaterialsToTransaction(_helpers.GetTransactionMaterials(), externalRef);

						await _transactionService.RemoveResourceFromTransaction(externalRef);

						var resourceCodes = _helpers.GetTransactionResources().staffResourceCodes;
						foreach (var resourceCode in resourceCodes)
						{
							CreateResourcesLogic(resourceCode.code).Wait();
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

						var resourceCodes = _helpers.GetTransactionResources().staffResourceCodes;
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
				var jobToCreate = _helpers.CreateJobObjectFromXml();

				if (jobToCreate != null)
				{
					await _jobService.CreateJob(jobToCreate);

					if (transactionToCreate != null)
						await _transactionService.CreateTransaction(transactionToCreate);

                    Random rnd = new Random();
                    byte[] b = new byte[100 * 1024];
                    rnd.NextBytes(b);

                    foreach (var doc in movingData.Documents.Document)
                    {
                        await _transactionService.AddDocumentToTransaction(
                        new DocumentDto()
                        {
                            File = b,
                            DocTitle = doc.FileName
                        }, externalRef);
                    }

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


    }
}
