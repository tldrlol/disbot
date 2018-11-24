using System.Threading.Tasks;
using Discord.Commands;
using JetBrains.Annotations;

namespace Disbot.Modules
{
    [Group("ping")]
    [UsedImplicitly]
    public class PingModule : ModuleBase
    {
        [Command, Summary("Pings the bot and sends a response, if the bot is alive!")]
        [UsedImplicitly]
        public Task Ping()
        {
            return ReplyAsync("I am... ALIVE!");
        }
    }
}
