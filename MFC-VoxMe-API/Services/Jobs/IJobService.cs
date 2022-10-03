using MFC_VoxMe_API.Dtos.Jobs;

namespace MFC_VoxMe_API.Services.Jobs
{
    public interface IJobService
    {
        Task<GetJobDetailsDto> GetDetails(string externalRef);
        Task<CreateJobDto> CreateJob(CreateJobDto createJobRequest);
    }
}
