using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.GlobalErrorHandling
{
    public class ExceptionMiddlewareExtensions
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddlewareExtensions(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Log.Error($"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var errorResponse = new ErrorDetails
            {
                Response = "Test"
            };
            switch (exception)
            {
                case ApplicationException e:
                    // custom application error
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorResponse.StatusCode = HttpStatusCode.Forbidden;
                    break;
                case NullReferenceException e:
                    // not found error
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    // unhandled error
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }

        //public static void UseHttpStatusCodeExceptionMiddleware(IApplicationBuilder app)
        //{
        //    app.UseMiddleware<ExceptionMiddlewareExtensions>();
        //}
    }
}
