using EasyNetQ;
using EShop.Api;
using EShop.Api.Consumers;
using EShop.Domain.Common.BrokerMessages;
using EShop.Repository;
using EShop.Service;
using EShop.Service.Interfaces;
using Hangfire;
using Hangfire.Common;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEShopRepositories(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddEShopServices(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddHostedService<TransactionState>();

/*
builder.Services.AddSingleton<IAsyncJobConsumer<TransactionMessage>, TransactionMessageConsumer>();

builder.Services.AddHostedService<TransactionMessageConsumerHost>();*/

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.UseHangfireServer();
app.UseHangfireDashboard("/hangfire");

app.Run();
