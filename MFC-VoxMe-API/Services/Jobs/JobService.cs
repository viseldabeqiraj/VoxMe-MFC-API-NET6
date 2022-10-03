using AutoMapper;
using MFC_VoxMe_API.Data;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Models;
using Newtonsoft.Json;
using System.Net;

namespace MFC_VoxMe_API.Services.Jobs
{
    public class JobService : IJobService
    {
        
        private readonly IConfiguration _config;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public JobService(DataContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }
        public string GetUrl(string query_string)
        {
            var url = _config.GetValue<string>("API_Url:url");
            url += query_string;
            return url;
        }

        //method to call the httpclient to get response from the url specified as a parameter
        public HttpResponseMessage MakeHttpCall(string url)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Fetch the JSON string from URL.
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(url).Result;
                return response;
            }
            catch(Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        public async Task<GetJobDetailsDto> GetDetails(string externalRef)
        {
            try
            {
                externalRef = "RS249955";

                var url = GetUrl($"/api/jobs/{externalRef}/details");
                GetJobDetailsDto jobDetails = new GetJobDetailsDto();

                var response = MakeHttpCall(url);
                if (response.IsSuccessStatusCode)
                {
                    jobDetails = JsonConvert.DeserializeObject<GetJobDetailsDto>(response.Content.ReadAsStringAsync().Result);
                }

                //Return the Deserialized JSON object.
                return jobDetails; 
            }
            catch(Exception ex)
            {

            }
            return null;
        }

        public Task<CreateJobDto> CreateJob(CreateJobDto createJobRequest)
        {
           try
            {
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

      
    }
}
