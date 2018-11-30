using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Disbot.Helpers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using JetBrains.Annotations;

namespace Disbot.Modules
{
    [Group("role")]
    public class RoleModule : ModuleBase
    {

        [Command("add"), Description("Adds a role to your user")]
        [UsedImplicitly]
        public Task Add(string roleName)
        {
            return TryCreateAndAssignRole(roleName, Color.Default);
        }

        [Command("add"), Description("Adds a role to your user")]
        [UsedImplicitly]
        public Task Add(string roleName, string roleColour)
        {
            var colour = StringToDiscordColourHelper.FromName(roleColour);

            return TryCreateAndAssignRole(roleName, colour);
        }

        [Command("add"), Description("Adds a role to your user")]
        [UsedImplicitly]
        public Task Add(string roleName, int rValue, int gValue, int bValue)
        {
            return Add(roleName, (float)rValue, (float)gValue, (float)bValue);
        }

        [Command("add"), Description("Adds a role to your user")]
        [UsedImplicitly]
        public Task Add(string roleName, float rValue, float gValue, float bValue)
        {
            var colour = new Color(rValue, gValue, bValue);

            return TryCreateAndAssignRole(roleName, colour);
        }

        [Command("assign"), Description("Assign a role")]
        [UsedImplicitly]
        public async Task Assign(string roleName)
        {
            var guildRole = Context.Guild.Roles
                .Where(x => x.Id != Constants.NSFW_ROLE)
                .Where(x => string.Equals(x.Name, roleName, StringComparison.CurrentCultureIgnoreCase))
                .SingleOrDefault();

            if (guildRole == null)
            {
                await ReplyAsync("There is no role with this name :(");
                return;
            }

            var guildUser = (SocketGuildUser)Context.User;

            await guildUser.AddRoleAsync(guildRole);
        }

        [Command("unassign"), Alias("remove"), Description("Unassign a role")]
        [UsedImplicitly]
        public async Task Unassign(string roleName)
        {
            var guildRole = Context.Guild.Roles
                .Where(x => string.Equals(x.Name, roleName, StringComparison.CurrentCultureIgnoreCase))
                .SingleOrDefault();

            if (guildRole == null)
            {
                await ReplyAsync("There is no role with this name :(");
                return;
            }

            var guildUser = (SocketGuildUser)Context.User;

            await guildUser.RemoveRoleAsync(guildRole);
        }

        [Command("edit"), Alias("modify", "update", "recolour", "recolor", "paint"), Description("Updates a role's colour")]
        [UsedImplicitly]
        public async Task Edit(string roleColour)
        {
            var guildUser = (SocketGuildUser)Context.User;

            var firstRole = 
                guildUser
                .Roles
                .Where(x => x.Id != Constants.NSFW_ROLE)
                .FirstOrDefault();

            if (firstRole == default)
            {
                await ReplyAsync("You have no roles to edit :(");
                return;
            }

            var guildRole = Context.Guild.GetRole(firstRole.Id);

            var colour = StringToDiscordColourHelper.FromName(roleColour);

            await guildRole.ModifyAsync(role => role.Color = colour);
        }

        [Command("edit"), Alias("modify", "update", "recolour", "recolor", "paint"), Description("Updates a role's colour")]
        [UsedImplicitly]
        public Task Edit(string roleName, string roleColour)
        {
            var colour = StringToDiscordColourHelper.FromName(roleColour);

            return AttemptToEditRole(roleName, colour);
        }

        [Command("edit"), Alias("modify", "update", "recolour", "recolor", "paint"), Description("Updates a role's colour")]
        [UsedImplicitly]
        public Task Edit(string roleName, int rValue, int gValue, int bValue)
        {
            return Edit(roleName, (float)rValue, (float)gValue, (float)bValue);
        }

        [Command("edit"), Alias("modify", "update", "recolour", "recolor", "paint"), Description("Updates a role's colour")]
        [UsedImplicitly]
        public Task Edit(string roleName, float rValue, float gValue, float bValue)
        {
            var colour = new Color(rValue, gValue, bValue);

            return AttemptToEditRole(roleName, colour);
        }

        private async Task TryCreateAndAssignRole(string roleName, Color colour)
        {
            var hasRole = Context.Guild.Roles.Any(x => string.Equals(x.Name, roleName, StringComparison.CurrentCultureIgnoreCase));

            if (hasRole)
            {
                await ReplyAsync("A role with this name already exists :(");
                return;
            }

            var newRole = await Context.Guild.CreateRoleAsync(roleName, GuildPermissions.None, colour);

            var guildUser = (SocketGuildUser)Context.User;

            await guildUser.AddRoleAsync(newRole);
        }

        private async Task AttemptToEditRole(string roleName, Color colour)
        {
            var guildUser = (IGuildUser)Context.User;

            var guildRole = Context.Guild.Roles.SingleOrDefault(x =>
                string.Equals(x.Name, roleName, StringComparison.CurrentCultureIgnoreCase));

            if (guildRole == null)
            {
                await ReplyAsync("No role with this name exists!");
                return;
            }

            if (guildRole.Id == Constants.NSFW_ROLE)
            {
                return;
            }

            if (!guildUser.RoleIds.Any(x => x == guildRole.Id))
            {
                await ReplyAsync("You don't own this role :(");
                return;
            }

            await guildRole.ModifyAsync(role => role.Color = colour);
        }
    }
}
