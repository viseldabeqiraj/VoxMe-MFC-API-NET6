using Serilog;
using System.Net;

namespace MFC_VoxMe_API.Logging
{
    public class LoggingHelper
    {
        public static void InsertLogs(string EndpointUrl, HttpResponseMessage response)
        {
            var result = response.Content.ReadAsStringAsync().Result;

            Log.Warning($"API Endpoint {EndpointUrl} returned {response.StatusCode} status code." +
                       $" Json Response: {result}");
        }
    }
}
