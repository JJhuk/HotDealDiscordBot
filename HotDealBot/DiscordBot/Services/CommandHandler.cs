using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotDealBot.DiscordBot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _command;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public CommandHandler(IServiceProvider serviceProvider)
        {
            _command = serviceProvider.GetRequiredService<CommandService>();
            _client = serviceProvider.GetRequiredService<DiscordSocketClient>();
            _configuration = serviceProvider.GetRequiredService<IConfiguration>();
            _serviceProvider = serviceProvider;

            _command.CommandExecuted += CommandExecutedAsync;
            _client.MessageReceived += OnClientMessage;
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
            {
                await context.Channel.SendMessageAsync($"{context.Message}");
                return;
            }

            if (result.IsSuccess)
            {
                Console.WriteLine($"Command [{command.Value.Name}] executed for -> [{context.User.Username}]");
                return;
            }

            await context.Channel.SendMessageAsync($"Sorry, ... something went wrong -> [{result}]!");
        }

        public async Task InitializeAsync()
        {
            await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        }

        private async Task OnClientMessage(IDeletable arg)
        {
            if (arg is not SocketUserMessage message)
            {
                return;
            }

            if (message.Author.IsBot)
            {
                return;
            }

            var pos = 0;
            var prefix = char.Parse(_configuration["Prefix"]);
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref pos) || message.HasCharPrefix(prefix, ref pos)))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            await _command.ExecuteAsync(context, pos, _serviceProvider);
        }
    }
}