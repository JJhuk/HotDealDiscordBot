using System.Threading.Tasks;
using Discord.Commands;

namespace HotDealBot.DiscordBot.Modules
{
    public class ExampleCommands : ModuleBase
    {
        [Command("hello")]
        public async Task HelloCommand()
        {
            await ReplyAsync($"Hello World : {Context.User}");
        }
    }
}