using AutoMapper;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.HttpMethods;
using MFC_VoxMe_API.Logging;
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
                var response = await HttpRequests.MakePostHttpCall(url, data);

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

                var response = await HttpRequests.MakeGetHttpCall(url);

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

                var response = await HttpRequests.MakeGetHttpCall(url);

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

        public async Task<UpdateTransactionDto> UpdateJob(UpdateTransactionDto updateTransactionRequest)
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
                    LoggingHelper.InsertLogs("UpdateJob", className, response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method UpdateJob in {className} failed. Exception thrown :{ex.Message}");
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

                var response = await HttpRequests.MakeGetHttpCall(url);

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
    }
}
