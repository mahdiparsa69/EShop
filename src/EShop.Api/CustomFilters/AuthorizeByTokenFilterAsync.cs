using EShop.Service.Implementations;
using EShop.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace EShop.Api.CustomFilters
{
    public class AuthorizeByTokenFilterAsync : Attribute, IAsyncAuthorizationFilter
    {
        private readonly ITokenService _tokenService;
        public AuthorizeByTokenFilterAsync(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var headerToken = context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues accessToken);

            if (headerToken == false)
                context.HttpContext.Response.StatusCode = 401;

            string[] tokenString = accessToken.ToString().Split(" ");

            var token = tokenString[1];

            var isValidToken = _tokenService.IsTokenValid(token);

            if (isValidToken == false)
            {
                context.Result = new JsonResult(new { Message = "Token Validation Has Failed. Request Access Denied" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

            }
        }

        

    }
}
