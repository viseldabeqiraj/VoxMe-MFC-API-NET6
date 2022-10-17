using static MFC_VoxMe_API.HttpMethods.HttpRequests;

namespace MFC_VoxMe_API.HttpMethods
{
    public interface IHttpRequests
    {
         Task<Token> GetAccessToken();
        Task<HttpResponseMessage> MakeGetHttpCall(string url, HttpContent data);
        Task<HttpResponseMessage> MakePostHttpCall(string url, HttpContent? data, IFormFile? file);
        Task<HttpResponseMessage> MakePutHttpCall(string url, HttpContent data);
        Task<HttpResponseMessage> MakeDeleteHttpCall(string url, StringContent? data);
        Task<HttpResponseMessage> MakePatchHttpCall(string url, HttpContent data);
    }
}
