using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Service.Implementations;
using EShop.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Service
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEShopServices(this IServiceCollection services)
        {
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
        }
    }
}
