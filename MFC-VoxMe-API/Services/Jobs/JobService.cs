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
        public string apiUrl = "https://879adad6-d4b6-40cd-99a3-1fc126ce1fa4.mock.pstmn.io";
        
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public JobService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public List<MovingData> CallApi(string url) //Apiurl + endpoint
        {
            //Setting TLS 1.2 protocol
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Fetch the JSON string from URL.
            List<MovingData> customers = new List<MovingData>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                customers = JsonConvert.DeserializeObject<List<MovingData>>(response.Content.ReadAsStringAsync().Result);
            }

            //Return the Deserialized JSON object.
            return customers;
        }
    }
}
