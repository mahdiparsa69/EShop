using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace EShop.Service
{
    public class RedisConnectionConfigurations
    {
        public string Connection { get; set; }
        public int ConnectionCount { get; set; }
        public int DbNumber { get; set; }
    }
}
