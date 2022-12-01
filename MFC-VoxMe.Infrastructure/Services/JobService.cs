using AutoMapper;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Models;
using Newtonsoft.Json;
using System.Net;
using MFC_VoxMe_API.HttpMethods;
using System.Text;
using Serilog;
using Microsoft.Extensions.Configuration;
using MFC_VoxMe_API.Services.Jobs;
using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe_API.Logging;
using MFC_VoxMe.Infrastructure.HttpMethods.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace MFC_VoxMe.Infrastructure.Services
{
    public class JobService : IJobService
    {

        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpRequests _httpRequests;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public string className;
        public JobService(DataContext context, IMapper mapper, IConfiguration config, IServiceProvider serviceProvider)
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

        public async Task<HttpResponseDto<JobDetailsDto>> GetDetails(string externalRef)
        {
                var url = GetUrl($"jobs/{externalRef}/details");
                var result = await GetHelperService<JobDetailsDto>()
                       .GetRequestHelper(url, null);

                return result;            
        }


        public async Task<HttpResponseDto<JobSummaryDto>> GetSummary(string externalRef)
        {
                var url = GetUrl($"jobs/{externalRef}/summary");

                var result = await GetHelperService<JobSummaryDto>()
                        .GetRequestHelper(url, null);

                return result;
        }


        public async Task<HttpResponseDto<CreateJobDto>> CreateJob(CreateJobDto request)
        {
                var url = GetUrl($"jobs");
                var result = await GetHelperService<CreateJobDto>()
                                   .PostRequestHelper(url, request);

                return result;           
        }


        public async Task<HttpResponseDto<UpdateJobDto>> UpdateJob(UpdateJobDto request, string externalRef)
        {
                var url = GetUrl($"jobs/{externalRef}");
                var result = await GetHelperService<UpdateJobDto>()
                                  .PutRequestHelper(url, request);

                return result;
        }

    }
}
