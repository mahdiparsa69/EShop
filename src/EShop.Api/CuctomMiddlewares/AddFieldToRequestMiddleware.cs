namespace EShop.Api.CuctomMiddlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AddFieldToRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public AddFieldToRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var isValidClientSecret = httpContext.Request.Headers.TryGetValue("X-ClientSecret", out var clientSecret);

            if (isValidClientSecret == false)
            {
                httpContext.Response.StatusCode = 404;

                await httpContext.Response.WriteAsync("Access is not valid ");
            }


            if (String.IsNullOrWhiteSpace(clientSecret.ToString()))
            {
                httpContext.Response.StatusCode = 404;

                await httpContext.Response.WriteAsync("Access is not valid ");
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AddFieldToRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseAddFieldToRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AddFieldToRequestMiddleware>();
        }
    }
}
