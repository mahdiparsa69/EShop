using EShop.Service.Interfaces;
using StackExchange.Redis;
using Newtonsoft.Json;
using IDatabase = StackExchange.Redis.IDatabase;

namespace EShop.Service.Implementations
{
    public class RedisCacheService : IRedisCacheService
    {
        //private readonly IDatabase _redisDb;
        private readonly IDatabase _redisDb;

        public RedisCacheService(IDatabase database)
        {
            _redisDb = database;
        }

        public async Task<T> FetchAsync<T>(string key)
        {
            var value = await _redisDb.StringGetAsync(key);

            if (!string.IsNullOrEmpty(value))
                return JsonConvert.DeserializeObject<T>(value);

            return default;
        }

        public async Task<T> FetchAsync<T>(string key, Func<Task<T>> getObjectFromStorageFunc, TimeSpan expiration)
        {
            var value = await _redisDb.StringGetAsync(key);

            if (value == RedisValue.Null)
            {
                var data = await getObjectFromStorageFunc();

                if (data != null)
                {
                    await StoreAsync(key, data, expiration);
                    return data;
                }

            }

            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<bool> StoreAsync<T>(string key, T value, TimeSpan expireTime)
        {
            var x = value;
            var y = JsonConvert.SerializeObject(value);

            var isSet = await _redisDb.StringSetAsync(key, JsonConvert.SerializeObject(value), expireTime);

            return isSet;
        }

        public async Task<bool> RemoveCacheAsync(string key)
        {
            bool isKeyExist = await _redisDb.KeyExistsAsync(key);
            if (isKeyExist == true)
                return await _redisDb.KeyDeleteAsync(key);

            return false;
        }

        public async Task<bool> HasKeyAsync(string key)
        {
            var value = await _redisDb.KeyExistsAsync(key);

            if (value == false)
                return false;

            return true;
        }
    }
}
