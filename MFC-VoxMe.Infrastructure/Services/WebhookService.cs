using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Core.Interfaces;
using MFC_VoxMe.Infrastructure.HttpMethods.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace MFC_VoxMe.Infrastructure.Services
{
    public class WebhookService : IWebhookService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public WebhookService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }
        public IRequestHelpers<T> GetHelperService<T>()
        {
            return _serviceProvider.GetService<IRequestHelpers<T>>();
        }

        public string GetUrl(string query_string)
        {
            return _configuration.GetValue<string>("API_Url:WebhookUrl") + query_string;
        }

        public async Task<HttpResponseDto<string>> PostVoxmeStatus(string job_number, string move_id, string status)
        {
            var url = GetUrl($"voxmestatus");
            var queryParams = new Dictionary<string, string>()
            {
            {"job_number", job_number },
            {"move_id", move_id },
            {"status", status }
            };

            url = QueryHelpers.AddQueryString(url, queryParams);
            var result = await GetHelperService<string>()
                   .GetRequestHelper(url,null);

            return result;
        }

    }
}
