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

            if (response.StatusCode == HttpStatusCode.NotFound)
                Log.Warning(warningLog(methodName, className, HttpStatusCode.NotFound, result));

            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                Log.Warning(warningLog(methodName, className, HttpStatusCode.Unauthorized, result));

            else if (response.StatusCode == HttpStatusCode.InternalServerError)
                Log.Warning(warningLog(methodName, className, HttpStatusCode.InternalServerError, result));

            else if (response.StatusCode == HttpStatusCode.Forbidden)
                Log.Warning(warningLog(methodName, className, HttpStatusCode.Forbidden, result));

            else if (response.StatusCode == HttpStatusCode.Conflict)
                Log.Warning(warningLog(methodName, className, HttpStatusCode.Conflict, result));
        }
    }
}
