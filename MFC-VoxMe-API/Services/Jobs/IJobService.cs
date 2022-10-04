using MFC_VoxMe_API.Dtos.Jobs;

namespace MFC_VoxMe_API.Services.Jobs
{
    public interface IJobService
    {
        Task<JobDetailsDto> GetDetails(string externalRef);
        Task<CreateJobDto> CreateJob(CreateJobDto createJobRequest);
        Task<JobSummaryDto> GetSummary(string externalRef);
        Task<CreateJobDto> UpdateJob(UpdateJobDto updateJobRequest);

     }
}
