using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Disbot.Helpers;
using Disbot.Models;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Serilog;

namespace Disbot.Modules
{
    public class NaughtyModule : ModuleBase
    {
        private const string BASE_URL = "https://api.redtube.com/?data=redtube.Videos.searchVideos&output=json&search={0}";

        private static readonly Random Random = new Random();

        [Command("naughty"), Alias("porn")]
        [UsedImplicitly]
        public async Task Naughty([Remainder]string searchValue)
        {
            if (Context.Channel.Id != Constants.NSFW_CHANNEL)
            {
                Log.Information("User {userName} attempted to run naughty command in the wrong channel", Context.User.Mention);
                await ReplyAsync("Naughty Boy! That's the wrong channel");                 
                return;
            }

            var user = (IGuildUser)Context.User;

            if (!user.RoleIds.Any(x => x == Constants.NSFW_ROLE))
            {
                Log.Information("User {userName} attempted to run naughty command with no 18+ role", Context.User.Mention);
                await ReplyAsync("Naughty Boy! You don't have the correct role to run this command");
                return;
            }

            try
            {
                using(var client = new HttpClient())
                {
                    var response = await client.GetAsync(string.Format(BASE_URL, searchValue));

                    var content = await response.Content.ReadAsStringAsync();

                    var deserialized = JsonConvert.DeserializeObject<NaughtyModel>(content);

                    if (deserialized == null || deserialized.Videos.Length <= 0)
                    {
                        await ReplyAsync($"Couldn't find anything for {searchValue}");
                        return;
                    }

                    var randomIndex = Random.Next(0, deserialized.Videos.Length - 1);

                    var videos = deserialized.Videos[randomIndex];

                    await ReplyAsync(videos.Video.Url.ToString());
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed getting naughty video with search term {searchTerm}", searchValue);
            }
        }
    }
}