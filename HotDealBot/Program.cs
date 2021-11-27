// See https://aka.ms/new-console-template for more information

using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HotDealBot.DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfiguration config;

ServiceProvider ConfigureServices() => new ServiceCollection()
    .AddSingleton(config)
    .AddSingleton<DiscordSocketClient>()
    .AddSingleton<CommandService>()
    .AddSingleton<CommandHandler>()
    .BuildServiceProvider();


async Task Main()
{
    var configurationBuilder = new ConfigurationBuilder();

    configurationBuilder
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("token.json");

    config = configurationBuilder.Build();

    await using var services = ConfigureServices();

    var client = services.GetRequiredService<DiscordSocketClient>();

    client.Log += LogAsync;

    services.GetRequiredService<CommandService>().Log += LogAsync;

    await client.LoginAsync(TokenType.Bot, config["Token"]);
    await client.StartAsync();

    await services.GetRequiredService<CommandHandler>().InitializeAsync();

    await Task.Delay(-1);
}

Task LogAsync(LogMessage log)
{
    Console.WriteLine(log.ToString());
    return Task.CompletedTask;
}

Main().GetAwaiter().GetResult();