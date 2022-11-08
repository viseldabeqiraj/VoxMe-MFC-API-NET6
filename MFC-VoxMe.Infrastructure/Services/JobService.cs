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

namespace MFC_VoxMe.Infrastructure.Services
{
    public class JobService : IJobService
    {

        private readonly IConfiguration _config;
        private readonly IHttpRequests _httpRequests;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public string className;
        public JobService(DataContext context, IMapper mapper, IConfiguration config, IHttpRequests httpRequests)
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

        public async Task<HttpResponseDto<JobDetailsDto>> GetDetails(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"jobs/{externalRef}/details");
                JobDetailsDto jobDetails = new JobDetailsDto();

                var response = await _httpRequests.MakeGetHttpCall(url, null);
                var result = new HttpResponseDto<JobDetailsDto>();
                result.responseStatus = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    jobDetails = JsonConvert.DeserializeObject<JobDetailsDto>(response.Content.ReadAsStringAsync().Result);
                    result.dto = jobDetails;
                }
                else
                {
                    LoggingHelper.InsertLogs("GetDetails", className, response);
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Method GetDetails in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }


        public async Task<HttpResponseDto<JobSummaryDto>> GetSummary(string externalRef)
        {
          //  try
           // {
                //externalRef = "RM002900";

                var url = GetUrl($"jobs/{externalRef}/summary");
                JobSummaryDto jobSummary = new JobSummaryDto();

                var response = await _httpRequests.MakeGetHttpCall(url, null);
                var result = new HttpResponseDto<JobSummaryDto>();
                result.responseStatus = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    jobSummary = JsonConvert.DeserializeObject<JobSummaryDto>(response.Content.ReadAsStringAsync().Result);
                    
                    throw new ApplicationException("GetSummary:" + className + response + "statuscode: " , new Exception(response.StatusCode.ToString()));
                    result.dto = jobSummary;
                }
                else
                {
                    LoggingHelper.InsertLogs("GetSummary", className, response);
                }
                return result;
           // }
            //catch (Exception ex)
            //{
            //    Log.Error($"Method GetSummary in {className} failed. Exception thrown :{ex.Message}");
            //}
            //return null;
        }



        public async Task<HttpResponseDto<CreateJobDto>> CreateJob(CreateJobDto createJobRequest)
        {
            try
            {
                var url = GetUrl($"jobs");
                var json = JsonConvert.SerializeObject(createJobRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePostHttpCall(url, data, null);
                var result = new HttpResponseDto<CreateJobDto>();
                result.responseStatus = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    result.dto = createJobRequest;
                }
                else
                {
                    LoggingHelper.InsertLogs("CreateJob", className, response);
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Method CreateJob in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }


        public async Task<HttpResponseDto<UpdateJobDto>> UpdateJob(UpdateJobDto updateJobRequest)
        {
            try
            {
                string externalRef = "RS249955";
                var url = GetUrl($"jobs/{externalRef}");
                var json = JsonConvert.SerializeObject(updateJobRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePutHttpCall(url, data);
                var result = new HttpResponseDto<UpdateJobDto>();
                result.responseStatus = response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    result.dto = updateJobRequest;
                }
                else
                {
                    LoggingHelper.InsertLogs("UpdateJob", className, response);
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error($"Method UpdateJob in {className} failed. Exception thrown :{ex.Message}");
                return null;
            }
        }


    }
}
