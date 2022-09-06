using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace EShop.Api.CuctomMiddlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HeaderLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HeaderLogMiddleware> _logger;

        public HeaderLogMiddleware(RequestDelegate next, ILogger<HeaderLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var accessToken = httpContext.Request.Headers.Authorization.ToString();

            var ipAddress = httpContext.Connection.RemoteIpAddress.ToString();

            var path = httpContext.Request.Host + httpContext.Request.Path + httpContext.Request.QueryString;

            _logger.LogInformation("***************Header:" + ipAddress);
            _logger.LogInformation("***************AccessToken:" + accessToken);
            _logger.LogInformation("***************Path:" + path);


            await _next(httpContext);

            // httpContext.Response.Headers.TryGetValue("Content-Type", out StringValues ContentType);

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HeaderLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseHeaderLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HeaderLogMiddleware>();
        }
    }
}
