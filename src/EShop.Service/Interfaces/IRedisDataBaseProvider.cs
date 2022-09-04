using StackExchange.Redis;

namespace EShop.Service.Interfaces
{
    public interface IRedisDataBaseProvider
    {
        IDatabase GetDatabase();
    }
}
