using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;

namespace MFC_VoxMe_API.Services.Jobs
{
    public interface IJobService
    {
        Task<HttpResponseDto<JobDetailsDto>> GetDetails(string externalRef);
        Task<HttpResponseDto<CreateJobDto>> CreateJob(CreateJobDto createJobRequest);
        Task<HttpResponseDto<JobSummaryDto>> GetSummary(string externalRef);
        Task<HttpResponseDto<UpdateJobDto>> UpdateJob(UpdateJobDto updateJobRequest, string externalRef);

     }
}
