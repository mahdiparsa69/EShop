using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace EShop.Repository.Implementations
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public bool Set(string key, string value)
        {
            var redisDb = _redis.GetDatabase();
            return redisDb.StringSet(key, value);
        }

        public string Get(string key)
        {
            var redisDb = _redis.GetDatabase();
            return redisDb.StringGet(key);
        }
    }
}
