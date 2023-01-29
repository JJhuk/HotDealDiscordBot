using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace HotDealBot;

public class InteractionHandler
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _handler;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;

    public InteractionHandler(
        DiscordSocketClient discordSocketClient,
        InteractionService handler,
        IServiceProvider services,
        IConfiguration configuration)
    {
        _client = discordSocketClient;
        _handler = handler;
        _services = services;
        _configuration = configuration;
    }
        
    public async Task Initialize()
    {
        _client.Ready += ReadyAsync;
        _handler.Log += log =>
        {
            Console.WriteLine(log);
            return Task.CompletedTask;
        };
        
        // InteractionModuleBase<T> 상속한 public 모듈들을 InteractionService에 추가
        await _handler.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        // Process the InteractionCreated payloads to execute Interactions commands
        _client.InteractionCreated += HandleInteraction;
    }
        
    private static bool IsDebug()
    {
#if DEBUG
        return true;
#else
            return false;
#endif
    }
        
    private async Task ReadyAsync()
    {
        // Context & 슬래쉬 커맨드들은 자동적으로 등록되지만, 클라이언트가 Ready 상태 이후에 진행됩니다.
        // 글로벌 커맨드는 1시간 후에 등록되고, 테스트 길드로 커맨드를 테스트합니다.
        if (IsDebug())
            await _handler.RegisterCommandsToGuildAsync(ulong.Parse(_configuration.GetSection("testGuild").Value!));
        else
            await _handler.RegisterCommandsGloballyAsync();
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
            var context = new SocketInteractionContext(_client, interaction);

            // Execute the incoming command.
            var result = await _handler.ExecuteCommandAsync(context, _services);

            if (!result.IsSuccess)
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    default:
                        break;
                }
        }
        catch
        {
            // If Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
            // response, or at least let the user know that something went wrong during the command execution.
            if (interaction.Type is InteractionType.ApplicationCommand)
                await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
    }
}