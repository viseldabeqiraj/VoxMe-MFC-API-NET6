﻿using MFC_VoxMe.Infrastructure.HttpMethods.AccessToken;
using MFC_VoxMe_API.BusinessLogic.AccessToken;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MFC_VoxMe_API.HttpMethods
{
    public class HttpRequests : IHttpRequests
    {
        private readonly IAccessTokenConfig _accessTokenConfig;
        private readonly AccessTokenConfigDto _accessTokenConfigDto;
        private readonly string className;
        public HttpClient client = new HttpClient();

        public HttpRequests(IAccessTokenConfig accessTokenConfig)
        {
            _accessTokenConfig = accessTokenConfig;
            _accessTokenConfigDto = accessTokenConfig.GetAccessTokenConfig();
            this.className = this.GetType().Name;
        }


        public async Task<Token> GetAccessToken()
        {
             var accessTokenUrl = _accessTokenConfigDto.accessTokenUrl;

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(accessTokenUrl),
                };

                var keyValues = new List<KeyValuePair<string, string>>();
                keyValues.Add(new KeyValuePair<string, string>("grant_type", _accessTokenConfigDto.grant_type));
                keyValues.Add(new KeyValuePair<string, string>("client_id", _accessTokenConfigDto.client_id));
                keyValues.Add(new KeyValuePair<string, string>("client_secret", _accessTokenConfigDto.client_secret));
                keyValues.Add(new KeyValuePair<string, string>("scope", _accessTokenConfigDto.scope));

                request.Content = new FormUrlEncodedContent(keyValues);
                HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                var jsonContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Token token = JsonConvert.DeserializeObject<Token>(jsonContent);
                    return token;
                }
                throw new ApplicationException(accessTokenUrl + " Status code: " + response.StatusCode + " " + response);            

        }

        //GET method to call the httpclient to get response from the url specified as a parameter
        public async Task<HttpResponseMessage> MakeGetHttpCall(string url, HttpContent? data)
        {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                    Content = data
                };
                HttpResponseMessage response;              
                    
                    request.Headers.Authorization = new AuthenticationHeaderValue(
                        "Bearer", GetAccessToken().Result.AccessToken.ToString()); 
                
                    response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                    return response;

                else
                    throw new ApplicationException
                    (url + " Status code: " + response.StatusCode + " " + response.Content.ReadAsStringAsync().Result);

        }

        //POST method by calling httpclient to post data on api side
        public async Task<HttpResponseMessage> MakePostHttpCall(string url, HttpContent? data, IFormFile? file)
        {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (file != null && file.Length > 0)
                {
                    byte[] fileData;
                    using (var br = new BinaryReader(file.OpenReadStream()))
                        fileData = br.ReadBytes((int)file.OpenReadStream().Length);

                    ByteArrayContent bytes = new ByteArrayContent(fileData);

                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    multiContent.Add(bytes, "file", file.FileName);

                    HttpResponseMessage fileResponse  = client.PostAsync(url, multiContent).Result;
                    return fileResponse;
                }
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = data
                };
                HttpResponseMessage response;

                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer", GetAccessToken().Result.AccessToken.ToString());

                response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                    return response;  
                
                else
                    throw new ApplicationException
                    (url + " Status code: " + response.StatusCode + " " + response.Content.ReadAsStringAsync().Result);
         
        }

        //PUT method by calling httpclient to update data on api side
        public async Task<HttpResponseMessage> MakePutHttpCall(string url, HttpContent data)
        {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(url),
                    Content = data
                };
                HttpResponseMessage response;

                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer", GetAccessToken().Result.AccessToken.ToString());

                response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                    return response;

                else
                throw new ApplicationException
                (url + " Status code: " + response.StatusCode + " " + response.Content.ReadAsStringAsync().Result);          

        }

        //DELETE method by calling httpclient to delete data on api side
        public async Task<HttpResponseMessage> MakeDeleteHttpCall(string url, StringContent? data)
        {
            
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url),
                    Content = data
                };
             
                HttpResponseMessage response;

                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer", GetAccessToken().Result.AccessToken.ToString());

                response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                    return response;

                else
                    throw new ApplicationException
                    (url + " Status code: " + response.StatusCode + " " + response.Content.ReadAsStringAsync().Result);
        }

        //PATCH method by calling httpclient
        public async Task<HttpResponseMessage> MakePatchHttpCall(string url, HttpContent data)
        {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Fetch the JSON string from URL.
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = new Uri(url),
                    Content = data
                };
                HttpResponseMessage response;

                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer", GetAccessToken().Result.AccessToken.ToString());

                response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                    return response;

                else
                    throw new ApplicationException
                    (url + " Status code: " + response.StatusCode + " " + response.Content.ReadAsStringAsync().Result);

        }       
    }
}
