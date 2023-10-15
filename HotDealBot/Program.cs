using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using HotDealBot.CommandHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfiguration config;
IServiceProvider services;

await Main();
return;

async Task Main()
{
    ConfigureServices();

    var client = services.GetRequiredService<DiscordSocketClient>();
    client.Log += LogAsync;
    client.Ready += ClientReady;

    var token = config.GetRequiredSection("Token").Value;

    if (token is null)
    {
        throw new InvalidOperationException("Discord Bot Token is not configured");
    }

    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();
    await services.GetRequiredService<CommandHandler>().InitializeAsync();

    // 무한루프
    await Task.Delay(Timeout.Infinite);
}

async Task ClientReady()
{
    var guildIdSection = config.GetRequiredSection("GuildId").Value;
    if (guildIdSection == null)
    {
        throw new InvalidOperationException("GuildId is not configured");
    }

    var value = ulong.Parse(guildIdSection);

    var interactionService = services.GetRequiredService<InteractionService>();

    // todo global
    await interactionService.RegisterCommandsToGuildAsync(value);
}

Task LogAsync(LogMessage log)
{
    Console.WriteLine(log.ToString());
    return Task.CompletedTask;
}

void ConfigureServices()
{
    var socketConfig = new DiscordSocketConfig()
    {
        AlwaysDownloadUsers = true,
        MessageCacheSize = 100,
        GatewayIntents = GatewayIntents.AllUnprivileged,
        LogLevel = LogSeverity.Info,
        UseInteractionSnowflakeDate = false,
    };

    config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName)
        .AddJsonFile("token.json")
        .Build();

    services = new ServiceCollection()
        .AddSingleton(config)
        .AddSingleton(socketConfig)
        .AddSingleton<DiscordSocketClient>()
        .AddSingleton<CommandHandler>()
        .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
        .BuildServiceProvider();
}
