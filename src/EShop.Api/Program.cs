using EShop.Repository;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEShopRepositories(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddAutoMapper(typeof(Program));
var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetSection("RedisConnection").Value);

builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
