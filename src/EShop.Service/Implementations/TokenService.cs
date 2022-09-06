using EShop.Domain.Common;
using EShop.Domain.Enums;
using EShop.Service.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EShop.Service.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string BuildToken(Object Payload, JwtHashAlgorithm hashAlgorithm)
        {

            var generatedToken = JwtHelper.Encode(Payload, _configuration["Jwt:Key"].ToString(), hashAlgorithm);

            return generatedToken;
        }

        public bool IsTokenValid(string Token)
        {
            var decodedPayload = JwtHelper.Decode<EShopAccessTokenPayload>(Token);

            if (decodedPayload == null)
                return false;

            if (DateTime.Now > decodedPayload.ExpireTokenTime)
                return false;

            var isValid = JwtHelper.Validate(Token, _configuration["Jwt:Key"].ToString());

            return isValid;
        }
    }
}
