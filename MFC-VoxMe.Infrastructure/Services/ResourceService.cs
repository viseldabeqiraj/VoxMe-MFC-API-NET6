using AutoMapper;
using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Infrastructure.HttpMethods.Helpers;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.HttpMethods;
using MFC_VoxMe_API.Logging;
using MFC_VoxMe_API.Services.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Text;

namespace MFC_VoxMe.Infrastructure.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpRequests _httpRequests;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public string className;
        public ResourceService(DataContext context, IMapper mapper, IConfiguration config, IServiceProvider serviceProvider)
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

        public async Task<HttpResponseDto<CreateResourceDto>> CreateResource(CreateResourceDto request)
        {

                var url = GetUrl($"management/resources");
                var result = await GetHelperService<CreateResourceDto>()
                         .PostRequestHelper(url, null, request);

                return result;
        }

        public async Task<HttpResponseDto<UpdateResourceDto>> UpdateResource(UpdateResourceDto request, string code)
        {
            
                var url = GetUrl($"management/resources/{code}");
                var result = await GetHelperService<UpdateResourceDto>()
                         .PutRequestHelper(url, request);

                return result;
        }

        public async Task<HttpResponseDto<bool>> DeleteResource(string code)
        {
            
            var url = GetUrl($"management/resources/{code}");
            var result = await GetHelperService<bool>()
                        .DeleteRequestHelper(url, null);

            return result;

        }
        //TODO:
        public async Task<HttpResponseDto<bool>> DisableResource(string code)
        {
            
                var url = GetUrl($"management/resources/{code}/disable");
                var result = await GetHelperService<bool>()
                                        .PatchRequestHelper(url, null);

                return result;
        }

        public async Task<HttpResponseDto<GetResourceDetailsDto>> GetDetails(string code)
        {
           
                var url = GetUrl($"management/resources/{code}");
                var result = await GetHelperService<GetResourceDetailsDto>()
                                .GetRequestHelper(url, null);

                return result;
        }

        //TODO: with request helpers
        public async Task<HttpResponseDto<ConfiguredMaterialsDto>> GetConfiguredMaterials(ResourceCodesForTransactionDto codes)
        {
            try
            {
                var url = GetUrl($"management/resources/materials");
                var json = JsonConvert.SerializeObject(codes);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpRequests.MakeGetHttpCall(url, data);
                ConfiguredMaterialsDto resourceDetails = new ConfiguredMaterialsDto();

                var result = new HttpResponseDto<ConfiguredMaterialsDto>();
                result.responseStatus = response.StatusCode;
                if (response != null)
                {
                    resourceDetails = JsonConvert.DeserializeObject<ConfiguredMaterialsDto>(response.Content.ReadAsStringAsync().Result);
                    result.dto = resourceDetails;
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Method GetConfiguredMaterials in {className} failed. Exception thrown :{ex.Message}");
                return null;
            }
        }

        //TOODO
        public async Task<HttpResponseDto<bool>> ForceConfigurationChanges(string appType)
        {
           
                var url = GetUrl($"management/configuration/download-to-devices?appType={appType}");

                var result = await GetHelperService<bool>()
                          .PutRequestHelper(url, null);

                return result;
            }
    }
}
