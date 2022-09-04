using EShop.Service.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace EShop.Service.Implementations
{
    public class RedisDataBaseProvider : IRedisDataBaseProvider
    {
        private readonly List<ConnectionMultiplexer> _connectionPool;
        private readonly IOptions<RedisConnectionConfigurations> _options;
        private readonly Random _random;
        private const int ConnectionCount = 15;

        //Ctor
        public RedisDataBaseProvider(IOptions<RedisConnectionConfigurations> options)
        {
            _random = new Random();
            _connectionPool = new List<ConnectionMultiplexer>();
            _options = options;

            for (int i = 0; i < _options.Value.ConnectionCount; i++)
            {
                var config = ConfigurationOptions.Parse(_options.Value.Connection);
                config.ConnectRetry = 10;
                config.ConnectTimeout = 5000;
                config.DefaultDatabase = _options.Value.DbNumber;
                config.AbortOnConnectFail = false;
                config.ReconnectRetryPolicy = new ExponentialRetry(1000);
                _connectionPool.Add(ConnectionMultiplexer.Connect(config));
            }

        }
        public IDatabase GetDatabase()
        {
            return _connectionPool[_random.Next(0, ConnectionCount)].GetDatabase();
        }
    }
}
