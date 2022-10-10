using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MFC_VoxMe_API.HttpMethods
{
    public class HttpRequests
    {
        //TODO: Authorization header needed for some requests

        //GET method to call the httpclient to get response from the url specified as a parameter
        public static async Task<HttpResponseMessage> MakeGetHttpCall(string url, HttpContent data)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpClient client = new HttpClient();

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url),
                    Content = data
                };

                HttpResponseMessage response = await client.GetAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error($"Method MakeGetHttpCall in HttpRequests failed. Exception thrown :{ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        //POST method by calling httpclient to post data on api side
        public static async Task<HttpResponseMessage> MakePostHttpCall(string url, HttpContent? data, IFormFile? file)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpClient client = new HttpClient();

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
                HttpResponseMessage response = await client.PostAsync(url, data);
                return response;
               
                }
                catch (Exception ex)
                {
                Log.Error($"Method MakePostHttpCall in HttpRequests failed. Exception thrown :{ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

        }

        //PUT method by calling httpclient to update data on api side
        public static async Task<HttpResponseMessage> MakePutHttpCall(string url, HttpContent data)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Fetch the JSON string from URL.
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PutAsync(url, data);
                //var result = await response.Content.ReadAsStringAsync();
                return response;//result;
            }
            catch (Exception ex)
            {
                Log.Error($"Method MakePutHttpCall in HttpRequests failed. Exception thrown :{ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        //DELETE method by calling httpclient to delete data on api side
        public static async Task<HttpResponseMessage> MakeDeleteHttpCall(string url, StringContent? data, bool needsAuth)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpClient client = new HttpClient();


                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url),
                    Content = data
                };

                HttpResponseMessage response;
                if (needsAuth) //TODO : option 2: replace with token value as string and check if token is not null
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "token val"); //TODO: be replaced
                    response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                }
                else
                 response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

               // HttpResponseMessage response = await client.DeleteAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error($"Method MakeDeleteHttpCall in HttpRequests failed. Exception thrown :{ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }
        //PATCH method by calling httpclient
        public static async Task<HttpResponseMessage> MakePatchHttpCall(string url, HttpContent data)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Fetch the JSON string from URL.
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PatchAsync(url, data);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error($"Method MakePatchHttpCall in HttpRequests failed. Exception thrown :{ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

    }
}
