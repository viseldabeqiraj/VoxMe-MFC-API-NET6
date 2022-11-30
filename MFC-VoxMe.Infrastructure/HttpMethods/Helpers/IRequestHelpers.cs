using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Core.Dtos.Transactions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.HttpMethods.Helpers
{
    public interface IRequestHelpers<T> 
    {
        Task<HttpResponseDto<T>> PostRequestHelper<T>(string url, IFormFile? file, T dto);
        Task<HttpResponseDto<T>> GetRequestHelper(string url,  HttpContent data);
        Task<HttpResponseDto<T>> PutRequestHelper<T>(string url, T? dto);
        Task<HttpResponseDto<bool>> DeleteRequestHelper(string url, StringContent? data);
        Task<HttpResponseDto<bool>> PatchRequestHelper(string url, StringContent? data);
        Task<HttpResponseDto<DocumentDto>> PostByteRequestHelper(string url, DocumentDto document);
        Task<HttpResponseDto<byte[]>> GetByteRequestHelper(string url);
    }
}
