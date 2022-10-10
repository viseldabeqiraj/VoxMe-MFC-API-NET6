using AutoMapper;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.HttpMethods;
using MFC_VoxMe_API.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Text;

namespace MFC_VoxMe_API.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public string className;

        public TransactionService(DataContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            this.className=this.GetType().Name;
        }
        public string GetUrl(string query_string)
        {
            var url = _config.GetValue<string>("API_Url:url");
            url += query_string;
            return url;
        }

        public async Task<CreateTransactionDto> CreateTransaction(CreateTransactionDto createTransactionRequest)
        {
            try
            {
                var url = GetUrl($"/api/transactions");
                var json = JsonConvert.SerializeObject(createTransactionRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await HttpRequests.MakePostHttpCall(url, data, null);

                if (response.IsSuccessStatusCode)
                {
                    return createTransactionRequest;
                }
                else
                {
                    LoggingHelper.InsertLogs("CreateTransaction", className, response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method CreateTransaction in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<TransactionDetailsDto> GetDetails(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/api/transactions/{externalRef}/details");
                TransactionDetailsDto transactionDetails = new TransactionDetailsDto();

                var response = await HttpRequests.MakeGetHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    transactionDetails = JsonConvert.DeserializeObject<TransactionDetailsDto>(response.Content.ReadAsStringAsync().Result);
                    return transactionDetails;
                }
                else
                {
                    LoggingHelper.InsertLogs("GetDetails", className, response);
                    return null;
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Method GetDetails in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<TransactionSummary> GetSummary(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/api/transactions/{externalRef}/summary");
                TransactionSummary transactionDetails = new TransactionSummary();

                var response = await HttpRequests.MakeGetHttpCall(url,null);

                if (response.IsSuccessStatusCode)
                {
                    transactionDetails = JsonConvert.DeserializeObject<TransactionSummary>(response.Content.ReadAsStringAsync().Result);
                    return transactionDetails;
                }
                else
                {
                    LoggingHelper.InsertLogs("GetSummary", className, response);
                    return null;
                }
            }

            catch (Exception ex)
            {
                Log.Error($"Method GetSummary in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<UpdateTransactionDto> UpdateTransaction(UpdateTransactionDto updateTransactionRequest)
        {
            try
            {
                string externalRef = "RS249955";
                var url = GetUrl($"/api/transactions/{externalRef}");
                var json = JsonConvert.SerializeObject(updateTransactionRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await HttpRequests.MakePutHttpCall(url, data);

                if (response.IsSuccessStatusCode)
                {
                    return updateTransactionRequest;
                }
                else
                {
                    LoggingHelper.InsertLogs("UpdateTransaction", className, response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method UpdateTransaction in {className} failed. Exception thrown :{ex.Message}");
                return null;
            }
        }

        public async Task<TransactionDownloadDetails> GetDownloadDetails(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/api/transactions/{externalRef}/download-details");
                TransactionDownloadDetails transactionDetails = new TransactionDownloadDetails();

                var response = await HttpRequests.MakeGetHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    transactionDetails = JsonConvert.DeserializeObject<TransactionDownloadDetails>(response.Content.ReadAsStringAsync().Result);
                    return transactionDetails;
                }
                else
                {
                    LoggingHelper.InsertLogs("GetDownloadDetails", className, response);
                    return null;
                }
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

                var url = GetUrl($"/api/transactions/{externalRef}");

                var response = await HttpRequests.MakeDeleteHttpCall(url, null, false);

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

                var url = GetUrl($"/api/transactions/{externalRef}/set-document");

                var response = await HttpRequests.MakePostHttpCall(url, null, File);
              
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
            catch(Exception ex)
            {
                Log.Error($"Method AddDocumentToTransaction in {className} failed. Exception thrown :{ex.Message}");
                return false;
            }
        }

        public async Task<AssignStaffDesignateForemanDto> AssignStaffDesignateForeman(AssignStaffDesignateForemanDto request, string externalRef)
        {
            try
            {
                var url = GetUrl($"/api/transactions/{externalRef}/crew");
                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await HttpRequests.MakePostHttpCall(url, data, null);

                if (response.IsSuccessStatusCode)
                {
                    return request;
                }
                else
                {
                    LoggingHelper.InsertLogs("CreateTransaction", className, response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method CreateTransaction in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        //TODO:
        public async Task<TransactionSummary> GetDocumentAsBinary(string EntityRef, string EntityType,string Name)
        {
            try
            {
                EntityRef = "RS249955";

                var url = GetUrl($"/api/documents?EntityRef={EntityRef}&EntityType={EntityType}&Name={Name}");

                var response = await HttpRequests.MakeGetHttpCall(url, null);

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
        public async Task<TransactionSummary> GetImageAsBinary(string EntityRef, string EntityType, string Name)
        {
            try
            {
                EntityRef = "RS249955";

                var url = GetUrl($"/api/images?EntityRef={EntityRef}&EntityType={EntityType}&Name={Name}");

                var response = await HttpRequests.MakeGetHttpCall(url, null);

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

        public async Task<bool> RemoveResourceFromTransaction(ResourceCodesForTransactionDto resourceCodes, string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/api/transactions/{externalRef}/resources");
                var json = JsonConvert.SerializeObject(resourceCodes);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await HttpRequests.MakeDeleteHttpCall(url, data,false);

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

        public async Task<ResourceCodesForTransactionDto> AssignResourcesToTransaction(ResourceCodesForTransactionDto request, string externalRef)
        {
            try
            {
                var url = GetUrl($"/api/transactions/{externalRef}/resources");
                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await HttpRequests.MakePostHttpCall(url, data, null);

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
                var url = GetUrl($"/api/transactions/{externalRef}/materials");
                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await HttpRequests.MakePostHttpCall(url, data, null);

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

        public async Task<bool> RemoveMaterialsFromTransaction(ResourceCodesForTransactionDto resourceCodes, string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/api/transactions/{externalRef}/materials");
                var json = JsonConvert.SerializeObject(resourceCodes);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await HttpRequests.MakeDeleteHttpCall(url, data, false);

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
