using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe.Core.Dtos.Email;
using MFC_VoxMe.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.GlobalErrorHandling
{
    public class ExceptionMiddlewareExtensions
    {

        private readonly RequestDelegate _next;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private EmailMessage? _emailConfiguration;

        public ExceptionMiddlewareExtensions(RequestDelegate next, IEmailService emailService, IConfiguration configuration)
        {
            _next = next;
            _emailService = emailService;
            _configuration = configuration;
        }
        public void EmailDataFromConfig()
        {
            _emailConfiguration = _configuration.GetSection
                ("EmailConfiguration:To").Get<EmailMessage>();
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
                case SqlException s:
                    EmailDataFromConfig();
                    var emailData = new EmailMessage()
                    {
                        Name = _emailConfiguration.Name,
                        Body = s.Number + s.Message,
                        EmailId = _emailConfiguration.EmailId,
                        Subject ="Sql Exception MFC Middleware"
                    };
                     _emailService.SendEmail(emailData);
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
