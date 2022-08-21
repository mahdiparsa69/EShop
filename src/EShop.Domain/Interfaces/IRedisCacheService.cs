using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.Interfaces
{
    public interface IRedisCacheService
    {
        string Get(string key);
        bool Set(string key, string value);
    }
}
