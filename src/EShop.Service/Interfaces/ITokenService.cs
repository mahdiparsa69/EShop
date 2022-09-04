using EShop.Domain.Enums;

namespace EShop.Service.Interfaces
{
    public interface ITokenService
    {
        string BuildToken(Object payload, JwtHashAlgorithm hashAlgorithm);

        bool IsTokenValid(string Token, string Key);
    }
}
