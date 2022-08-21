using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Interfaces;
using EShop.Repository.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace EShop.Repository
{
    public static class ServiceCollectionExtensions
    {
        // public static IConfigurationRoot Configuration;
        public static void AddEShopRepositories(this IServiceCollection services, IConfigurationSection configuration)
        {
            services.AddDbContextPool<EShopDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetSection("EShopDatabase").Value);

                options.UseSnakeCaseNamingConvention();

                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
            }, 1024);

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IOrderRepository, OrderRepository>();




            //services.AddAutoMapper(typeof(Program));


        }
    }
}
