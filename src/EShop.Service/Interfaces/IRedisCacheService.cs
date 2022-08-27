namespace EShop.Service.Interfaces
{
    public interface IRedisCacheService
    {
        //todo add comment

        Task<T> FetchAsync<T>(string key);

        Task<T> FetchAsync<T>(string key, Func<Task<T>> getObjectFromStorageFunc, TimeSpan expiration);

        Task<bool> StoreAsync<T>(string key, T value, TimeSpan expireTime);

        Task<bool> RemoveCacheAsync(string key);

        //todo implement
        Task<bool> HasKeyAsync(string key);
    }
}
