using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ConsumerRequestLog.Consumer;
using EShop.Domain.Common.BrokerMessages;
using EShop.Domain.Models;
using EShop.Service.Interfaces;
using Microsoft.Extensions.Primitives;

namespace EShop.Api.CuctomMiddlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HeaderLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HeaderLogMiddleware> _logger;
        //private readonly IAsyncJobProducer _asyncJobProducer;

        public HeaderLogMiddleware(RequestDelegate next, ILogger<HeaderLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            //_asyncJobProducer = asyncJobProducer;
        }

        public async Task Invoke(HttpContext httpContext, IAsyncJobProducer _asyncJobProducer)
        {
            var accessToken = httpContext.Request.Headers.Authorization.ToString();

            var ipAddress = httpContext.Connection.RemoteIpAddress.ToString();

            var path = httpContext.Request.Host + httpContext.Request.Path + httpContext.Request.QueryString;

            httpContext.Response.Headers.TryGetValue("Content-Type", out StringValues ContentType);

            RequestLogMessage requestLog = new RequestLogMessage()
            {
                Path = path,
                AccessToken = accessToken,
                IpAddress = ipAddress,
                ContentType = ContentType
            };
            //await _asyncJobProducer.PublishAsync(requestLog, httpContext.RequestAborted);

            await _next(httpContext);

            await _asyncJobProducer.PublishAsync(requestLog, httpContext.RequestAborted);
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
