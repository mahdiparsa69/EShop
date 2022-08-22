using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Interfaces;
using EShop.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace EShop.Service.Implementations
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        /*public async Task<bool> Set(string key, string value)
        {
            var redisDb = _redis.GetDatabase();
            return await redisDb.StringSetAsync(key, value);
        }

        public async Task<string> Get(string key)
        {
            var redisDb = _redis.GetDatabase();
            return await redisDb.StringGetAsync(key);
        }
*/
        public async Task<T> FetchAsync<T>(string key)
        {
            var redisDb = _redis.GetDatabase();

            var value = await redisDb.StringGetAsync(key);

            if (!string.IsNullOrEmpty(value))
                return JsonConvert.DeserializeObject<T>(value);

            return default;
        }

        public async Task<T> FetchAsync<T>(string key, Func<Task<T>> getObjectFromStorageFunc, TimeSpan expiration)
        {
            var redisDb = _redis.GetDatabase();

            var value = await redisDb.StringGetAsync(key);

            if (value == RedisValue.Null)
            {
                var data = await getObjectFromStorageFunc();

                if (data != null)
                {
                    await StoreAsync(key, data, expiration);
                    return data;
                }

            }

            return default;
        }

        public async Task<bool> StoreAsync<T>(string key, T value, TimeSpan expireTime)
        {
            //TimeSpan expireTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var redisDb = _redis.GetDatabase();

            var isSet = await redisDb.StringSetAsync(key, JsonConvert.SerializeObject(value), expireTime);

            return isSet;
        }

        public async Task<bool> RemoveCacheAsync(string key)
        {
            var redisDb = _redis.GetDatabase();

            bool isKeyExist = await redisDb.KeyExistsAsync(key);
            if (isKeyExist == true)
                return await redisDb.KeyDeleteAsync(key);

            return false;
        }

        /*public async Task<T> GetDataFromDb<T>(Func<T, bool> whereFunc)
        {
            IQueryable<T> query = db.Employee.Where(x => whereFunc(x));
            var employee = await query.ToListAsync();
            return employee;
        }*/
    }
}
