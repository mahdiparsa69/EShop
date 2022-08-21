using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interfaces
{
    public interface IRedisCacheService
    {
        /// <summary>
        /// Get Data From Redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> FetchAsync<T>(string key);

        /// <summary>
        /// Store Data in Redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        Task<bool> StoreAsync<T>(string key, T value, DateTimeOffset expirationTime);
    }
}
