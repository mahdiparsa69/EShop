using EShop.Api.CuctomMiddlewares;
using EShop.Api.CustomFilters;
using EShop.Repository;
using EShop.Service;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEShopRepositories(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddEShopServices(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<AuthorizeByTokenFilterAsync>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCustomeExceptionHandlerMiddleware();

app.UseHeaderLogMiddleware();

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
