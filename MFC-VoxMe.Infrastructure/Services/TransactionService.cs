using AutoMapper;
using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.HttpMethods;
using MFC_VoxMe_API.Logging;
using MFC_VoxMe_API.Services.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace MFC_VoxMe.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IConfiguration _config;
        private readonly IHttpRequests _httpRequests;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public readonly string className;

        public TransactionService(DataContext context, IMapper mapper, IConfiguration config, IHttpRequests httpRequests)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _httpRequests = httpRequests;
            className = GetType().Name;
        }
        public string GetUrl(string query_string)
        {
            var url = _config.GetValue<string>("API_Url:url");
            url += query_string;
            return url;
        }

        public async Task<HttpResponseDto<CreateTransactionDto>> CreateTransaction(CreateTransactionDto createTransactionRequest)
        {
            try
            {
                var url = GetUrl($"api/transactions");
                var json = JsonConvert.SerializeObject(createTransactionRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePostHttpCall(url, data, null);

                var result = new HttpResponseDto<CreateTransactionDto>();
                result.responseStatus = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    result.dto = createTransactionRequest;
                }           
                else
                {
                    LoggingHelper.InsertLogs("CreateTransaction", className, response);
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Method CreateTransaction in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<HttpResponseDto<TransactionDetailsDto>> GetDetails(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/mfc/v2/transactions/{externalRef}/details");
                TransactionDetailsDto transactionDetails = new TransactionDetailsDto();

                var response = await _httpRequests.MakeGetHttpCall(url, null);
                var result = new HttpResponseDto<TransactionDetailsDto>();
                result.responseStatus = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    transactionDetails = JsonConvert.DeserializeObject<TransactionDetailsDto>(response.Content.ReadAsStringAsync().Result);
                    result.dto = transactionDetails;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    LoggingHelper.InsertLogs("GetDetails", className, response);
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Method GetDetails in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<HttpResponseDto<TransactionSummaryDto>> GetSummary(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/mfc/v2/transactions/{externalRef}/summary");
                TransactionSummaryDto transactionSummary = new TransactionSummaryDto();

                var response = await _httpRequests.MakeGetHttpCall(url, null);
                var result = new HttpResponseDto<TransactionSummaryDto>();
                result.responseStatus=response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    transactionSummary = JsonConvert.DeserializeObject<TransactionSummaryDto>(response.Content.ReadAsStringAsync().Result);
                    result.dto = transactionSummary;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    LoggingHelper.InsertLogs("GetSummary", className, response);
                }
                return result;

            }

            catch (Exception ex)
            {
                Log.Error($"Method GetSummary in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<HttpResponseDto<UpdateTransactionDto>> UpdateTransaction(UpdateTransactionDto updateTransactionRequest)
        {
            try
            {
                string externalRef = "RS249955";
                var url = GetUrl($"/mfc/v2/transactions/{externalRef}");
                var json = JsonConvert.SerializeObject(updateTransactionRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePutHttpCall(url, data);

                var result = new HttpResponseDto<UpdateTransactionDto>();
                result.responseStatus = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    result.dto = updateTransactionRequest;
                }
                else
                {
                    LoggingHelper.InsertLogs("UpdateTransaction", className, response);
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Method UpdateTransaction in {className} failed. Exception thrown :{ex.Message}");
                return null;
            }
        }

        public async Task<HttpResponseDto<TransactionDownloadDetails>> GetDownloadDetails(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/mfc/v2/transactions/{externalRef}/download-details");
                TransactionDownloadDetails transactionDetails = new TransactionDownloadDetails();

                var response = await _httpRequests.MakeGetHttpCall(url, null);
                var result = new HttpResponseDto<TransactionDownloadDetails>();
                result.responseStatus = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    transactionDetails = JsonConvert.DeserializeObject<TransactionDownloadDetails>(response.Content.ReadAsStringAsync().Result);
                    result.dto = transactionDetails;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    LoggingHelper.InsertLogs("GetDownloadDetails", className, response);
                }
                return result;

            }

            catch (Exception ex)
            {
                Log.Error($"Method GetDownloadDetails in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<bool> DeleteTransaction(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/mfc/v2/transactions/{externalRef}");

                var response = await _httpRequests.MakeDeleteHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    LoggingHelper.InsertLogs("DeleteTransaction", className, response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method DeleteTransaction in {className} failed. Exception thrown :{ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddDocumentToTransaction(IFormFile File, string DocTitle, string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/mfc/v2/transactions/{externalRef}/set-document");

                var response = await _httpRequests.MakePostHttpCall(url, null, File);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    LoggingHelper.InsertLogs("CreateTransaction", className, response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method AddDocumentToTransaction in {className} failed. Exception thrown :{ex.Message}");
                return false;
            }
        }

        public async Task<HttpResponseDto<AssignStaffDesignateForemanDto>> AssignStaffDesignateForeman(AssignStaffDesignateForemanDto request, string externalRef)
        {
            try
            {
                var url = GetUrl($"/mfc/v2/transactions/{externalRef}/crew");
                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePostHttpCall(url, data, null);

                var result = new HttpResponseDto<AssignStaffDesignateForemanDto>();
                result.responseStatus = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    result.dto = request;
                }
                else
                {
                    LoggingHelper.InsertLogs("AssignStaffDesignateForeman", className, response);
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Method CreateTransaction in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        //TODO:
        public async Task<TransactionSummaryDto> GetDocumentAsBinary(string EntityRef, string EntityType, string Name)
        {
            try
            {
                EntityRef = "RS249955";

                var url = GetUrl($"/mfc/v2/documents?EntityRef={EntityRef}&EntityType={EntityType}&Name={Name}");

                var response = await _httpRequests.MakeGetHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    //TODO:
                }
                else
                {
                    LoggingHelper.InsertLogs("GetDocumentAsBinary", className, response);
                    return null;
                }
            }

            catch (Exception ex)
            {
                Log.Error($"Method GetDocumentAsBinary in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }
        //TODO:
        public async Task<TransactionSummaryDto> GetImageAsBinary(string EntityRef, string EntityType, string Name)
        {
            try
            {
                EntityRef = "RS249955";

                var url = GetUrl($"/mfc/v2/images?EntityRef={EntityRef}&EntityType={EntityType}&Name={Name}");

                var response = await _httpRequests.MakeGetHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    //TODO:
                }
                else
                {
                    LoggingHelper.InsertLogs("GetImageAsBinary", className, response);
                    return null;
                }
            }

            catch (Exception ex)
            {
                Log.Error($"Method GetImageAsBinary in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<bool> RemoveResourceFromTransaction(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/mfc/v2/transactions/{externalRef}/resources");
                var response = await _httpRequests.MakeDeleteHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    LoggingHelper.InsertLogs("RemoveResourceFromTransaction", className, response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method RemoveResourceFromTransaction in {className} failed. Exception thrown :{ex.Message}");
                return false;
            }
        }
        //Will not be used
        public async Task<ResourceCodesForTransactionDto> AssignResourcesToTransaction(ResourceCodesForTransactionDto request, string externalRef)
        {
            try
            {
                var url = GetUrl($"/mfc/v2/transactions/{externalRef}/resources");
                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePostHttpCall(url, data, null);

                if (response.IsSuccessStatusCode)
                {
                    return request;
                }
                else
                {
                    LoggingHelper.InsertLogs("AssignResourcesToTransaction", className, response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method AssignResourcesToTransaction in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<AssignMaterialsToTransactionDto> AssignMaterialsToTransaction(AssignMaterialsToTransactionDto request, string externalRef)
        {
            try
            {
                externalRef = "RS9192314";
                var url = GetUrl($"/mfc/v2/transactions/{externalRef}/materials");
                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePostHttpCall(url, data, null);

                if (response.IsSuccessStatusCode)
                {
                    return request;
                }
                else
                {
                    LoggingHelper.InsertLogs("AssignMaterialsToTransaction", className, response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method AssignMaterialsToTransaction in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<bool> RemoveMaterialsFromTransaction(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/mfc/v2/transactions/{externalRef}/materials");
                var response = await _httpRequests.MakeDeleteHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    LoggingHelper.InsertLogs("RemoveMaterialsFromTransaction", className, response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method RemoveMaterialsFromTransaction in {className} failed. Exception thrown :{ex.Message}");
                return false;
            }
        }
    }
}
