using Discord.Interactions;
using HotDealBot.CommandHandlers;

namespace HotDealBot.Modules;

public class ExampleCommands : InteractionModuleBase<SocketInteractionContext>
{
    private readonly InteractionService commands;
    private readonly CommandHandler commandHandler;

    public ExampleCommands(
        InteractionService commands,
        CommandHandler commandHandler)
    {
        this.commands = commands;
        this.commandHandler = commandHandler;
    }

    [SlashCommand("ping", "pong!")]
    public async Task Ping()
    {
        await RespondAsync("pong!");
    }
}
