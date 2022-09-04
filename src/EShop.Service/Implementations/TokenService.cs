using EShop.Domain.Enums;
using EShop.Service.Interfaces;

namespace EShop.Service.Implementations
{
    public class TokenService : ITokenService
    {

        public string BuildToken(Object payload, string key, JwtHashAlgorithm hashAlgorithm)
        {
            var generatedToken = JwtHelper.Encode(payload, key, hashAlgorithm);

            /* if (generatedToken != null)
             {
                 HttpContext.Session.SetString("Token", generatedToken);
             }
             var validToken = JwtHelper.Validate(generatedToken, key);*/

            return generatedToken;
        }

        public bool IsTokenValid(string Token, string Key)
        {
            var isValid = JwtHelper.Validate(Token, Key);

            return isValid;
        }
    }
}
