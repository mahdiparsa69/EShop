using EShop.Service.Implementations;
using EShop.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace EShop.Service
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEShopServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisConnectionConfigurations>(configuration.GetSection("RedisConnection"));

            services.AddSingleton<IRedisDataBaseProvider, RedisDataBaseProvider>();

            services.AddTransient<IDatabase>(provider => provider.GetRequiredService<IRedisDataBaseProvider>().GetDatabase());

            services.AddScoped<IRedisCacheService, RedisCacheService>();
        }
    }
}
