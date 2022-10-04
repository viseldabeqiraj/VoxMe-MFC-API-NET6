using System.Net;

namespace MFC_VoxMe_API.HttpMethods
{
    public class HttpRequests
    {
        //GET method to call the httpclient to get response from the url specified as a parameter
        public static async Task<HttpResponseMessage> MakeGetHttpCall(string url)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Fetch the JSON string from URL.
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        //POST method by calling httpclient to post data on api side
        public static async Task<HttpResponseMessage> MakePostHttpCall(string url, HttpContent data)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Fetch the JSON string from URL.
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(url, data);
                //var result = await response.Content.ReadAsStringAsync();
                return response;//result;
            }
            catch (Exception ex)
            {
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
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        //DELETE method by calling httpclient to delete data on api side
        public static async Task<HttpResponseMessage> MakeDeleteHttpCall(string url)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Fetch the JSON string from URL.
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(url);
                //var result = await response.Content.ReadAsStringAsync();
                return response;//result;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

    }
}
