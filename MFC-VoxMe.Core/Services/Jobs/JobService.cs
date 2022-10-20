using AutoMapper;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Models;
using Newtonsoft.Json;
using System.Net;
using MFC_VoxMe_API.HttpMethods;
using System.Text;
using Serilog;
using MFC_VoxMe_API.Logging;
using Microsoft.Extensions.Configuration;

namespace MFC_VoxMe_API.Services.Jobs
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
            this.className = this.GetType().Name;
        }
        public string GetUrl(string query_string)
        {
            var url = _config.GetValue<string>("API_Url:url");
            url += query_string;
            return url;
        }
   
        public async Task<JobDetailsDto> GetDetails(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/mfc/v2/jobs/{externalRef}/details");
                JobDetailsDto jobDetails = new JobDetailsDto();

                var response = await _httpRequests.MakeGetHttpCall(url, null);

                if (response.IsSuccessStatusCode)
                {
                    jobDetails = JsonConvert.DeserializeObject<JobDetailsDto>(response.Content.ReadAsStringAsync().Result);
                    return jobDetails;
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


        public async Task<JobSummaryDto> GetSummary(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/mfc/v2/jobs/{externalRef}/summary");
                JobSummaryDto jobSummary = new JobSummaryDto();

                var response = await _httpRequests.MakeGetHttpCall(url, null);
                if (response.IsSuccessStatusCode)
                {
                    jobSummary = JsonConvert.DeserializeObject<JobSummaryDto>(response.Content.ReadAsStringAsync().Result);
                    return jobSummary;
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

 

        public async Task<CreateJobDto> CreateJob(CreateJobDto createJobRequest)
        {
           try
            {
                var url = GetUrl($"/mfc/v2/jobs");
                var json = JsonConvert.SerializeObject(createJobRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePostHttpCall(url, data, null);

                if (response.IsSuccessStatusCode)
                {
                    return createJobRequest;
                }
                else
                {
                    LoggingHelper.InsertLogs("CreateJob", className, response);
                    return null;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"Method CreateJob in {className} failed. Exception thrown :{ex.Message}");
            }
            return null;
        }


        public async Task<UpdateJobDto> UpdateJob(UpdateJobDto updateJobRequest)
        {
            try
            {
                string externalRef = "RS249955";
                var url = GetUrl($"/mfc/v2/jobs/{externalRef}");
                var json = JsonConvert.SerializeObject(updateJobRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpRequests.MakePutHttpCall(url, data);

                if (response.IsSuccessStatusCode)
                {
                    return updateJobRequest;
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

       
    }
}
