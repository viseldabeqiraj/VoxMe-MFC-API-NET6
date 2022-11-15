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
                Log.Error(ex.Message + "\n" + ex.StackTrace);
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var errorResponse = new ErrorDetails
            {
                Response = exception.Message
            };
            switch (exception)
            {
                case ApplicationException e:
                    // custom application error
                    if (exception.Message.Contains("Status code: BadRequest"))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse.StatusCode = HttpStatusCode.BadRequest;
                        break;
                    }
                    else if (exception.Message.Contains("Status code: Unauthorized"))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        errorResponse.StatusCode = HttpStatusCode.Unauthorized;
                        break;
                    }
                    else if (exception.Message.Contains("Status code: Conflict"))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                        errorResponse.StatusCode = HttpStatusCode.Conflict;
                        break;
                    }
                    else if (exception.Message.Contains("Status code: Forbidden"))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.StatusCode = HttpStatusCode.Forbidden;
                        break;
                    }
                    else if (exception.Message.Contains("Status code: NotFound"))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        errorResponse.StatusCode = HttpStatusCode.NotFound;
                        break;
                    }
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.StatusCode = HttpStatusCode.InternalServerError;
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
