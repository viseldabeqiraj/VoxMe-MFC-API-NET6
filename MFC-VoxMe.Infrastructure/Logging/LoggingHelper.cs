using Serilog;
using System.Net;

namespace MFC_VoxMe_API.Logging
{
    public class LoggingHelper
    {
        public static string warningLog(string methodName, string className, HttpStatusCode status, string data)
        {
            return $"Method {methodName} in {className} returned {status}." +
                       $" Json Response: {data}";
        }

        public static void InsertLogs(string methodName, string className, HttpResponseMessage response)
        {
            var result = response.Content.ReadAsStringAsync().Result;

            Log.Warning(warningLog(methodName, className, response.StatusCode, result));
        }
    }
}
