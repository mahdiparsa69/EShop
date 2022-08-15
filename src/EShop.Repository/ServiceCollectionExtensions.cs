using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Repository
{
    public static class ServiceCollectionExtensions
    {
        // public static IConfigurationRoot Configuration;
        public static IServiceCollection AddEShopRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<EShopDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
