using Microsoft.AspNetCore.Http;

namespace MFC_VoxMe.Infrastructure.GlobalErrorHandling
{
    public interface IExceptionMiddlewareExtensions
    {
        Task InvokeAsync(HttpContext httpContext);
    }
}