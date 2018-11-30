using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using JetBrains.Annotations;

namespace Disbot.Modules
{
    [Group("util")]
    [UsedImplicitly]
    public class UtilityModule : ModuleBase
    {
        [Command("roles")]
        [UsedImplicitly]
        public Task GetRoles()
        {
            var roles = Context.Guild.Roles.Select(x => $"{x.Id} {x.Name}");

            return ReplyAsync(string.Join(", ", roles));
        }
    }
}
