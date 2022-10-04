using AutoMapper;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Models;
using Newtonsoft.Json;
using System.Net;
using MFC_VoxMe_API.HttpMethods;
using System.Text;

namespace MFC_VoxMe_API.Services.Jobs
{
    public class JobService : IJobService
    {
        
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public JobService(DataContext context, IMapper mapper, IConfiguration config, ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
            _logger = logger;
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

                var url = GetUrl($"/api/jobs/{externalRef}/details");
                JobDetailsDto jobDetails = new JobDetailsDto();

                var response = await HttpRequests.MakeGetHttpCall(url);
                if (response.IsSuccessStatusCode)
                {
                    jobDetails = JsonConvert.DeserializeObject<JobDetailsDto>(response.Content.ReadAsStringAsync().Result);
                }
                _logger.LogError($"Method GetDetails in {this.GetType().Name} failed. Exception thrown");

                //Return the Deserialized JSON object.
                return jobDetails;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Method GetDetails in {this.GetType().Name} failed. Exception thrown: {ex.Message}");
            }
            return null;
        }

        public async Task<JobSummaryDto> GetSummary(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/api/jobs/{externalRef}/summary");
                JobSummaryDto jobSummary = new JobSummaryDto();

                var response = await HttpRequests.MakeGetHttpCall(url);
                if (response.IsSuccessStatusCode)
                {
                    jobSummary = JsonConvert.DeserializeObject<JobSummaryDto>(response.Content.ReadAsStringAsync().Result);
                }

                //Return the Deserialized JSON object.
                return jobSummary;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<HttpResponseMessage> CreateJob(CreateJobDto createJobRequest)
        {
           try
            {
                var url = GetUrl($"/api/jobs");
                var json = JsonConvert.SerializeObject(createJobRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await HttpRequests.MakePostHttpCall(url, data);
                return response;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> UpdateJob(UpdateJobDto updateJobRequest)
        {
            try
            {
                var url = GetUrl($"/api/jobs");
                var json = JsonConvert.SerializeObject(updateJobRequest);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await HttpRequests.MakePutHttpCall(url, data);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
