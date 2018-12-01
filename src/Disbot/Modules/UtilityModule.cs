using System;
using System.Linq;
using System.Threading.Tasks;
using Disbot.Helpers;
using Discord.Commands;
using JetBrains.Annotations;
using Serilog;

namespace Disbot.Modules
{
    [Group("util")]
    [UsedImplicitly]
    public class UtilityModule : ModuleBase
    {
        [Command("forcerole")]
        [UsedImplicitly]
        public async Task ForceRole()
        {
            try
            {
                var botInGuild = await Context.Guild.GetUserAsync(Context.Client.CurrentUser.Id);

                var nsfwRole = Context.Guild.GetRole(Constants.NSFW_ROLE);

                await botInGuild.AddRoleAsync(nsfwRole);
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed forcing 18 role");
            }
        }
    }
}
