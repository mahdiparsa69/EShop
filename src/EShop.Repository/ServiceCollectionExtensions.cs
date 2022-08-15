using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEShopRepository(this IServiceCollection services)
        {

            return services;
        }
    }
}
