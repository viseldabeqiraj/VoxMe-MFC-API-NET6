using AutoMapper;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.resources;
using MFC_VoxMe_API.HttpMethods;
using MFC_VoxMe_API.Logging;
using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace MFC_VoxMe_API.Services.Resources
{
    public class ResourceService : IResourceService
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public string className;
        public ResourceService(DataContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            this.className = this.GetType().Name;
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
                var url = GetUrl($"/api/management/resources");
                var json = JsonConvert.SerializeObject(createResourceRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await HttpRequests.MakePostHttpCall(url, data, null);

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
                var url = GetUrl($"/management/resources/{code}");
                var json = JsonConvert.SerializeObject(updateResourceRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await HttpRequests.MakePutHttpCall(url, data);

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
                var url = GetUrl($"/api/management/resources/{code}");

                var response = await HttpRequests.MakeDeleteHttpCall(url, null,false);

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
                var url = GetUrl($"/api/management/resources/{code}/disable");

                var response = await HttpRequests.MakePatchHttpCall(url, null);

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
    }
}
