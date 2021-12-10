using HotDealServer.Models;
using HotDealServer.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<HotDealDatabaseSettings>(builder.Configuration
        .GetSection(nameof(HotDealDatabaseSettings))) // HotDealDatabaseSettings라는 이름의 Configuration DI
    .AddSingleton<IHotDealDatabaseSettings>(serviceProvider => serviceProvider
        .GetRequiredService<IOptions<HotDealDatabaseSettings>>().Value)
    .AddSingleton<HotDealItemService>()
    .AddConnections();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Run();