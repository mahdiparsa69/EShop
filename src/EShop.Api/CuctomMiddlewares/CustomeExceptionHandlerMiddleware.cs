namespace EShop.Api.CuctomMiddlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomeExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomeExceptionHandlerMiddleware> _logger;
        public CustomeExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomeExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Occured : " + ex);

                httpContext.Response.StatusCode = 500;
                //httpContext.Response.Redirect("/Error");

            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomeExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomeExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomeExceptionHandlerMiddleware>();
        }
    }
}
