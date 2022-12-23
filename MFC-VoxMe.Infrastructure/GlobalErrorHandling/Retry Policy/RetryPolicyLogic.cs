using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Polly;
using Polly.Retry;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.GlobalErrorHandling.Retry_Policy
{
    public static class RetryPolicyLogic
    {
        private static readonly IEnumerable<TimeSpan> RetryTimes = new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(3),
            TimeSpan.FromSeconds(4),
            TimeSpan.FromSeconds(5)
        };

        private static readonly AsyncRetryPolicy RetryPolicy = Policy
                 .Handle<SqlException>(SqlServerTransientExceptionDetector.ShouldRetryOn)
                 .Or<TimeoutException>()
                 .Or<SqlException>()
                 .OrInner<Win32Exception>(SqlServerTransientExceptionDetector.ShouldRetryOn)
                 .WaitAndRetryAsync(RetryTimes,
                     (exception, timeSpan, retryCount, context) =>
                     {
                          Log.Error(
                            exception,
                            @$"ERROR: Error talking to Database, will retry after {timeSpan}. Retry attempt {retryCount}",
                            timeSpan,
                            retryCount
                     );});

        public static async Task<int> ExecuteAsyncWithRetry(this IDbConnection cnn,
                                                            string sql,
                                                            object? param = null) =>
            await RetryPolicy.ExecuteAsync(async () => await cnn.ExecuteAsync(sql, param));

       public  static async Task<dynamic> QueryAsyncWithRetry(this IDbConnection cnn,
                                                             string sql,
                                                             object? param = null) =>
            await RetryPolicy.ExecuteAsync(async () => await cnn.QueryAsync(sql, param));
    }
}
