using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace EShop.Service.Interfaces
{
    public interface IRedisDataBaseProvider
    {
        IDatabase GetDatabase();
    }
}
