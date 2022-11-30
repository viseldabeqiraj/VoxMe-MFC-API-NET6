using MFC_VoxMe.Core.Dtos.Transactions;
using MFC_VoxMe.Infrastructure.HttpMethods.AccessToken;
using Microsoft.AspNetCore.Http;
using static MFC_VoxMe_API.HttpMethods.HttpRequests;

namespace MFC_VoxMe_API.HttpMethods
{
    public interface IHttpRequests
    {
         Task<Token> GetAccessToken();
        Task<HttpResponseMessage> MakeGetHttpCall(string url, HttpContent data);
        Task<HttpResponseMessage> MakePostHttpCall(string url, HttpContent? data);
        Task<HttpResponseMessage> MakePutHttpCall(string url, HttpContent data);
        Task<HttpResponseMessage> MakeDeleteHttpCall(string url, StringContent? data);
        Task<HttpResponseMessage> MakePatchHttpCall(string url, HttpContent data);
        Task<HttpResponseMessage> GetBinaryStream(string url);
        Task<HttpResponseMessage> PostFile(string url, DocumentDto document);
    }
}
