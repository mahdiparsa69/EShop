// See https://aka.ms/new-console-template for more information

using ConsumerRequestLog.Consumer;
using EShop.Domain.Common.BrokerMessages;
using EShop.Repository;
using EShop.Service;
using EShop.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("befor");
/*await Host.CreateDefaultBuilder(args)
    .RunConsoleAsync();*/
//var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(app =>
    {
        app.AddJsonFile("appsettings.json");
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddEShopRepositories(hostContext.Configuration.GetSection("ConnectionStrings"));

        services.AddEShopServices(hostContext.Configuration);


        /*services.AddDbContextPool<EShopDbContext>(options =>
        {
            options.UseNpgsql(hostContext.Configuration.GetConnectionString("EShopDatabase"));

            options.UseSnakeCaseNamingConvention();

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
        }, 1024);*/

        services.AddSingleton<IAsyncJobConsumer<RequestLogMessage>, RequestLogConsumer>();

        services.AddHostedService<RequestLogConsumerHost>();
    })
    .RunConsoleAsync();