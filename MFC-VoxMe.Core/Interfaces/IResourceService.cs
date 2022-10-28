using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;

namespace MFC_VoxMe_API.Services.Resources
{
    public interface IResourceService
    {
        Task<CreateResourceDto> CreateResource(CreateResourceDto createResourceRequest);
        Task<UpdateResourceDto> UpdateResource(UpdateResourceDto updateResourceRequest, string code);
        Task<bool> DeleteResource(string code);
        Task<bool> DisableResource(string code);
        Task<GetResourceDetailsDto> GetDetails(string code);
        Task<ConfiguredMaterialsDto> GetConfiguredMaterials(ResourceCodesForTransactionDto codes);
        Task<bool> ForceConfigurationChanges(string appType);
    }
}
