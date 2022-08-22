using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Common;
using EShop.Domain.Interfaces;

namespace EShop.Service.Interfaces
{
    public interface IRedisCacheService
    {
        /// <summary>
        /// Get Data From Redis without getting data from db
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> FetchAsync<T>(string key);

        /// <summary>
        /// Get Data From Redis with getting data from db
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="getObjectFromStorageFunc"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        Task<T> FetchAsync<T>(string key, Func<Task<T>> getObjectFromStorageFunc, TimeSpan expiration);

        /// <summary>
        /// Store Data in Redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        Task<bool> StoreAsync<T>(string key, T value, TimeSpan expireTime);

        Task<bool> RemoveCacheAsync(string key);

    }
}
