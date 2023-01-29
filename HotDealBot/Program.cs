// See https://aka.ms/new-console-template for more information

using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HotDealBot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfiguration config;
IServiceProvider services;

void Configure()
{
    var socketConfig = new DiscordSocketConfig()
    {
        AlwaysDownloadUsers = true,
        MessageCacheSize = 100,
        GatewayIntents = GatewayIntents.AllUnprivileged,
        LogLevel = LogSeverity.Info
    };
    
    config = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("token.json")
        .Build();

    services = new ServiceCollection()
        .AddSingleton(config)
        .AddSingleton(socketConfig)
        .AddSingleton<DiscordSocketClient>()
        .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
        .AddSingleton<InteractionHandler>()
        .BuildServiceProvider();
}

async Task Main()
{
    Configure();

    var client = services.GetRequiredService<DiscordSocketClient>();
    client.Log += LogAsync;

    // 커맨드들 등록
    await services.GetRequiredService<InteractionHandler>()
        .Initialize();

    await client.LoginAsync(TokenType.Bot, config["Token"]);
    await client.StartAsync();
    
    // 무한루프
    await Task.Delay(-1);
}

Task LogAsync(LogMessage log)
{
    Console.WriteLine(log.ToString());
    return Task.CompletedTask;
}

Main().GetAwaiter().GetResult();