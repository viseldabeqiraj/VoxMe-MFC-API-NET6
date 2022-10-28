using AutoMapper;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.HttpMethods;
using MFC_VoxMe_API.Logging;
using MFC_VoxMe_API.Services.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Text;
using MFC_VoxMe_API.Logging;
using MFC_VoxMe_API.Dtos.Transactions;

namespace MFC_VoxMe.Infrastructure.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IConfiguration _config;
        private readonly IHttpRequests _httpRequests;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public string className;
        public ResourceService(DataContext context, IMapper mapper, IConfiguration config, IHttpRequests httpRequests)
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

        public async Task<CreateResourceDto> CreateResource(CreateResourceDto createResourceRequest)
        {
            try
            {
                var url = GetUrl($"/mfc/v2/management/resources");
                var json = JsonConvert.SerializeObject(createResourceRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePostHttpCall(url, data, null);

                if (response.IsSuccessStatusCode)
                {
                    return createResourceRequest;
                }
                else
                {
                    LoggingHelper.InsertLogs("CreateResource", className, response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method CreateResource in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }

        public async Task<UpdateResourceDto> UpdateResource(UpdateResourceDto updateResourceRequest, string code)
        {
            try
            {
                string externalRef = "RS249955";
                var url = GetUrl($"/mfc/v2/management/resources/{code}");
                var json = JsonConvert.SerializeObject(updateResourceRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePutHttpCall(url, data);

                if (response.IsSuccessStatusCode)
                {
                    return updateResourceRequest;
                }
                else
                {
                    LoggingHelper.InsertLogs("UpdateResource", className, response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method UpdateResource in {className} failed. Exception thrown :{ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteResource(string code)
        {
            try
            {
                var url = GetUrl($"/mfc/v2/management/resources/{code}");

                var response = await _httpRequests.MakeDeleteHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    LoggingHelper.InsertLogs("DeleteResource", className, response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method DeleteResource in {className} failed. Exception thrown :{ex.Message}");
                return false;
            }
        }
        //TODO:
        public async Task<bool> DisableResource(string code)
        {
            try
            {
                var url = GetUrl($"/mfc/v2/management/resources/{code}/disable");

                var response = await _httpRequests.MakePatchHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    LoggingHelper.InsertLogs("DisableResource", className, response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method DisableResource in {className} failed. Exception thrown :{ex.Message}");
                return false;
            }
        }

        public async Task<GetResourceDetailsDto> GetDetails(string code)
        {
            try
            {
                var url = GetUrl($"/mfc/v2/management/resources/{code}");

                var response = await _httpRequests.MakeGetHttpCall(url, null);
                GetResourceDetailsDto resourceDetails = new GetResourceDetailsDto();
                if (response.IsSuccessStatusCode)
                {
                    resourceDetails = JsonConvert.DeserializeObject<GetResourceDetailsDto>(response.Content.ReadAsStringAsync().Result);
                    return resourceDetails;
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
                return null;
            }
        }

        public async Task<ConfiguredMaterialsDto> GetConfiguredMaterials(ResourceCodesForTransactionDto codes)
        {
            try
            {
                var url = GetUrl($"/mfc/v2/management/resources/materials");
                var json = JsonConvert.SerializeObject(codes);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpRequests.MakeGetHttpCall(url, data);
                ConfiguredMaterialsDto resourceDetails = new ConfiguredMaterialsDto();
                if (response.IsSuccessStatusCode)
                {
                    resourceDetails = JsonConvert.DeserializeObject<ConfiguredMaterialsDto>(response.Content.ReadAsStringAsync().Result);
                    return resourceDetails;
                }
                else
                {
                    LoggingHelper.InsertLogs("GetConfiguredMaterials", className, response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method GetConfiguredMaterials in {className} failed. Exception thrown :{ex.Message}");
                return null;
            }
        }

        public async Task<bool> ForceConfigurationChanges(string appType)
        {
            try
            {
                var url = GetUrl($"/api/management/configuration/download-to-devices?appType={appType}");
               
                var response = await _httpRequests.MakePutHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    LoggingHelper.InsertLogs("ForceConfigurationChanges", className, response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method ForceConfigurationChanges in {className} failed. Exception thrown :{ex.Message}");
                return false;
            }
        }
    }
}
