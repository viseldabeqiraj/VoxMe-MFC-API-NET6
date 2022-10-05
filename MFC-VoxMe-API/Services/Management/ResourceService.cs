using AutoMapper;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.HttpMethods;
using MFC_VoxMe_API.Logging;
using Newtonsoft.Json;

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
        public async Task<bool> RemoveResourceFromTransaction(List<string> resourceCodes, string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/api/transactions/{externalRef}/details");

                var response = await HttpRequests.MakeDeleteHttpCall(url);

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
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
