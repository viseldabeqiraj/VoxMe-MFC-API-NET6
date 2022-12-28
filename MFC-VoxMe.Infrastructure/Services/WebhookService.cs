using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Core.Interfaces;
using MFC_VoxMe.Infrastructure.HttpMethods.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //https://edentraining.jkmoving.com:751/voxmestatus?job_number=RS0207777&move_id=47777&status=24
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
                   .PostRequestHelper(url, "");

            return result;
        }

    }
}
