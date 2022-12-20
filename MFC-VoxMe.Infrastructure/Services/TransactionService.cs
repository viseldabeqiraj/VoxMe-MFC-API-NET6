using AutoMapper;
using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Core.Dtos.Transactions;
using MFC_VoxMe.Infrastructure.HttpMethods;
using MFC_VoxMe.Infrastructure.HttpMethods.Helpers;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.HttpMethods;
using MFC_VoxMe_API.Logging;
using MFC_VoxMe_API.Services.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace MFC_VoxMe.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public readonly string className;
        public string apiUrl;

        public TransactionService(DataContext context, IMapper mapper, IConfiguration config, IServiceProvider serviceProvider)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _serviceProvider = serviceProvider;
            className = GetType().Name;
        }
        public IRequestHelpers<T> GetHelperService<T>()
        {
            return _serviceProvider.GetService<IRequestHelpers<T>>();
        }

        public string GetUrl(string query_string)
        {
            var url = _config.GetValue<string>("API_Url:url");
            url += query_string;
           return url;
        }

        //TODO:: replace output of each endpoint in service to void, because we already know that response is success in Httprequest class
        public async Task<HttpResponseDto<CreateTransactionDto>> CreateTransaction(CreateTransactionDto createTransactionRequest)
        {
            var url = GetUrl($"transactions");
            var result = await GetHelperService<CreateTransactionDto>()
                .PostRequestHelper(url, createTransactionRequest);

            return result;
        }

        public async Task<HttpResponseDto<TransactionDetailsDto>> GetDetails(string externalRef)
        {
            var url = GetUrl($"transactions/{externalRef}/details");
            var result = await GetHelperService<TransactionDetailsDto>()
                   .GetRequestHelper(url, null);

            return result;
        }

        public async Task<HttpResponseDto<TransactionSummaryDto>> GetSummary(string externalRef)
        {
            var url = GetUrl($"transactions/{externalRef}/summary");
            var result = await GetHelperService<TransactionSummaryDto>()
                   .GetRequestHelper(url, null);

            return result;

        }

        public async Task<HttpResponseDto<UpdateTransactionDto>> UpdateTransaction(string externalRef, UpdateTransactionDto updateTransactionRequest)
        {

            var url = GetUrl($"transactions/{externalRef}");
            var result = await GetHelperService<UpdateTransactionDto>()
            .PutRequestHelper(url, updateTransactionRequest);

            return result;

        }

        public async Task<HttpResponseDto<List<TransactionDownloadDetails>>> GetDownloadDetails(string externalRef)
        {

            var url = GetUrl($"transactions/{externalRef}/download-details");
            var result = await GetHelperService<List<TransactionDownloadDetails>>()
                   .GetRequestHelper(url, null);

            return result;
        }

        public async Task<HttpResponseDto<bool>> DeleteTransaction(string externalRef)
        {

            var url = GetUrl($"transactions/{externalRef}");

            var result = await GetHelperService<bool>()
                  .DeleteRequestHelper(url, null);

            return result;

        }

        public async Task<HttpResponseDto<DocumentDto>> AddDocumentToTransaction(DocumentDto document, string externalRef)
        {

            var url = GetUrl($"transactions/{externalRef}/set-document");
            var result = await GetHelperService<DocumentDto>()
                                .PostByteRequestHelper(url, document);

            return result;
        }


        public async Task<HttpResponseDto<AssignStaffDesignateForemanDto>> AssignStaffDesignateForeman(AssignStaffDesignateForemanDto request, string externalRef)
        {
            var url = GetUrl($"transactions/{externalRef}/crew");
            var result = await GetHelperService<AssignStaffDesignateForemanDto>()
                .PostRequestHelper(url, request);

            return result;
        }

        //TODO:
        public async Task<HttpResponseDto<byte[]>> GetDocumentAsBinary(string EntityRef, string EntityType, string Name)
        {

            var url = GetUrl($"documents?EntityRef={EntityRef}&EntityType={EntityType}&Name={Name}");

            var result = await GetHelperService<byte[]>()
                            .GetByteRequestHelper(url);
            return result;
        }
        //TODO:
        public async Task<HttpResponseDto<byte[]>> GetImageAsBinary(string EntityRef, string EntityType, string Name)
        {
            var url = GetUrl($"images?EntityRef=RS0150687&EntityType=Transaction&Name=signature_1666862783756.png");
            //var url = GetUrl($"images?EntityRef={EntityRef}&EntityType=Transaction&Name={name}");
            var result = await GetHelperService<byte[]>()
                            .GetByteRequestHelper(url);
            return result;

        }

        public async Task<HttpResponseDto<bool>> RemoveResourceFromTransaction(string externalRef)
        {
            externalRef = "RS249955";

            var url = GetUrl($"transactions/{externalRef}/resources");
            var result = await GetHelperService<bool>()
                             .DeleteRequestHelper(url, new StringContent(@"[]", Encoding.UTF8, "application/json"));
            return result;
        }
        //Will not be used
        public async Task<HttpResponseDto<ResourceCodesForTransactionDto>> AssignResourcesToTransaction(ResourceCodesForTransactionDto request, string externalRef)
        {

            var url = GetUrl($"transactions/{externalRef}/resources");
            var result = await GetHelperService<ResourceCodesForTransactionDto>()
                              .PostRequestHelper(url, request);
            return result;
        }

        public async Task<HttpResponseDto<AssignMaterialsToTransactionDto>> AssignMaterialsToTransaction(AssignMaterialsToTransactionDto request, string externalRef)
        {
            var url = GetUrl($"transactions/{externalRef}/materials");
            var result = await GetHelperService<AssignMaterialsToTransactionDto>()
                            .PostRequestHelper(url, request);
            return result;
        }

        public async Task<HttpResponseDto<bool>> RemoveMaterialsFromTransaction(string externalRef)
        {
            var url = GetUrl($"transactions/{externalRef}/materials");
            var result = await GetHelperService<bool>()
                             .DeleteRequestHelper(url, new StringContent(@"[]", Encoding.UTF8, "application/json"));
            return result;
        }
    }
}
