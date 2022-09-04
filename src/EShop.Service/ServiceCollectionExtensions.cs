using EasyNetQ;
using EShop.Service.Implementations;
using EShop.Service.Interfaces;
using Hangfire;
using Hangfire.PostgreSql;
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

            services.AddScoped<IAsyncJobProducer, AsyncJobProducer>();

            services.AddScoped<ITokenService, TokenService>();

            services.AddSingleton<IBus>(RabbitHutch.CreateBus("host=127.0.0.1,port=5672"));

            /* services.AddSingleton<IAsyncJobConsumer<TransactionMessage>, TransactionMessageConsumer>();

             services.AddHostedService<TransactionMessageConsumerHost>();*/

            services.AddHangfire(x => x
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(configuration.GetConnectionString("EShopDatabase")));
        }
    }
}
