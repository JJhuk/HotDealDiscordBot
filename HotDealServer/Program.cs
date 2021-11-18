using System;
using System.Threading.Tasks;
using HotDealServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PuppeteerSharp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddDbContext<HotDealDbContext>(options => options.UseInMemoryDatabase("Products"));

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Run();