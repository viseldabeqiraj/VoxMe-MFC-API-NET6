namespace MFC_VoxMe_API.Services.Resources
{
    public interface IResourceService
    {
        Task<bool> RemoveResourceFromTransaction(List<string> resourceCodes, string externalRef);
    }
}
