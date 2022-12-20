using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Core.Dtos.Transactions;
using MFC_VoxMe.Infrastructure.HttpMethods.Helpers;
using MFC_VoxMe_API.HttpMethods;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.HttpMethods
{
    public class RequestHelpers<T> : IRequestHelpers<T> 
    {
        private readonly IHttpRequests _httpRequests;

        public RequestHelpers(IHttpRequests httpRequests)
        {
            _httpRequests = httpRequests;
        }

        public async Task<HttpResponseDto<T>> PostRequestHelper<T>(string url, T dto) 
        {
            StringContent? data = null;
            if (dto != null)
            {
                var json = JsonConvert.SerializeObject(dto);
                data = new StringContent(json, Encoding.UTF8, "application/json");
            }
                var response = await _httpRequests.MakePostHttpCall(url, data);

                var result = new HttpResponseDto<T>();
                result.responseStatus = response.StatusCode;

                result.dto = dto;
                return result;
        }

        public async Task<HttpResponseDto<T>> GetRequestHelper(string url, HttpContent? data)
        {
            var response = await _httpRequests.MakeGetHttpCall(url, data);

            var result = new HttpResponseDto<T>();
            result.responseStatus = response.StatusCode;
            result.dto = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            
            return result;
        }

        public async Task<HttpResponseDto<T>> PutRequestHelper<T>(string url, T? dto) 
        {
            StringContent data = null;

            if (dto != null)
            {
                var json = JsonConvert.SerializeObject(dto);
                data = new StringContent(json, Encoding.UTF8, "application/json");
            }
            var response = await _httpRequests.MakePutHttpCall(url, data);

                var result = new HttpResponseDto<T>();
                result.responseStatus = response.StatusCode;

                result.dto = dto;
                return result;
        }

        public async Task<HttpResponseDto<bool>> DeleteRequestHelper(string url, StringContent? data) 
        {
            var response = await _httpRequests.MakeDeleteHttpCall(url, data);

                var result = new HttpResponseDto<bool>();
                result.responseStatus = response.StatusCode;
                return result;
        }

        public async Task<HttpResponseDto<bool>> PatchRequestHelper(string url, StringContent? data)
        {
            var response = await _httpRequests.MakePatchHttpCall(url, data);

            var result = new HttpResponseDto<bool>();
            result.responseStatus = response.StatusCode;
            return result;
        }
        public async Task<HttpResponseDto<byte[]>> GetByteRequestHelper(string url)
        {
            var response = await _httpRequests.MakeGetHttpCall(url, null);
            var bytes = response.Content.ReadAsByteArrayAsync().Result;
            var base64 = Convert.ToBase64String(bytes);
            var result = new HttpResponseDto<byte[]>()
            {
                dto = bytes
            };
            return result;
        }

        public async Task<HttpResponseDto<DocumentDto>> PostByteRequestHelper(string url, DocumentDto document)
        {
            var response = await _httpRequests.PostFile(url, document);

            var result = new HttpResponseDto<DocumentDto>();
            result.responseStatus = response.StatusCode;
            return result;
        }

        public async Task<HttpResponseDto<DocumentDto>> PostByteRequestHelper(string url, DocumentDto document)
        {
            var response = await _httpRequests.PostFile(url, document);

            var result = new HttpResponseDto<DocumentDto>();
            result.responseStatus = response.StatusCode;
            return result;
        }
        public async Task<HttpResponseDto<byte[]>> GetByteRequestHelper(string url)
        {
            var response = await _httpRequests.MakeGetHttpCall(url, null);
            var bytes = response.Content.ReadAsByteArrayAsync().Result;
            var base64 = Convert.ToBase64String(bytes);
            var result = new HttpResponseDto<byte[]>()
            {
                dto = bytes,
                responseStatus = response.StatusCode,
            };
            return result;
        }

    }
}
