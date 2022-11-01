using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;

namespace MFC_VoxMe_API.Services.Resources
{
    public interface IResourceService
    {
        Task<HttpResponseDto<CreateResourceDto>> CreateResource(CreateResourceDto createResourceRequest);
        Task<HttpResponseDto<UpdateResourceDto>> UpdateResource(UpdateResourceDto updateResourceRequest, string code);
        Task<bool> DeleteResource(string code);
        Task<bool> DisableResource(string code);
        Task<HttpResponseDto<GetResourceDetailsDto>> GetDetails(string code);
        Task<HttpResponseDto<ConfiguredMaterialsDto>> GetConfiguredMaterials(ResourceCodesForTransactionDto codes);
        Task<bool> ForceConfigurationChanges(string appType);
    }
}
