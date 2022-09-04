using System.Text;
using EasyNetQ;
using EShop.Api;
using EShop.Api.Consumers;
using EShop.Api.CuctomMiddlewares;
using EShop.Domain.Common.BrokerMessages;
using EShop.Repository;
using EShop.Service;
using EShop.Service.Interfaces;
using Hangfire;
using Hangfire.Common;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEShopRepositories(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddEShopServices(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSwaggerGen();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCustomeExceptionHandlerMiddleware();

//app.UseAddFieldToRequestMiddleware();

app.MapControllers();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V2");
});

app.UseHangfireServer();

app.UseHangfireDashboard("/hangfire");

app.Run();
