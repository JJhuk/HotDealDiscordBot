using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace HotDealBot.CommandHandlers;

public class CommandHandler
{
    private readonly DiscordSocketClient client;
    private readonly InteractionService commands;
    private readonly IServiceProvider services;

    public CommandHandler(DiscordSocketClient client,
        InteractionService commands,
        IServiceProvider services)
    {
        this.client = client;
        this.commands = commands;
        this.services = services;
    }

    public async Task InitializeAsync()
    {
        //  InteractionModuleBase<T> 타입들을  InteractionService에 추가한다.
        await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        client.InteractionCreated += HandleInteraction;
    }

    private async Task HandleInteraction(SocketInteraction arg)
    {
        try
        {
            // InteractionModuleBase<T> 모듈의 제네릭 파라메터에 맞는 실행 컨택스트 생성
            var ctx = new SocketInteractionContext(client, arg);
            await commands.ExecuteCommandAsync(ctx, services);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            if(arg.Type == InteractionType.ApplicationCommand)
            {
                await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}
