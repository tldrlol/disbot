using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Disbot.Helpers;
using Disbot.Models.Redtube;
using Disbot.Models.Reddit;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Serilog;

namespace Disbot.Modules
{
    [Group("Naughty")]
    [UsedImplicitly]
    public class NaughtyModule : ModuleBase
    {
        private const string REDTUBE_BASE_URL = "https://api.redtube.com/?data=redtube.Videos.searchVideos&output=json&search={0}";
        private const string REDDIT_API_BASE_URL = "https://www.reddit.com/r/porn_gifs/search.json?q={0}&restrict_sr=1&include_over_18=1/";

        private static readonly Random Random = new Random();

        [Command("video")]
        [UsedImplicitly]
        public async Task Naughty([Remainder] string searchValue)
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
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(string.Format(REDTUBE_BASE_URL, searchValue));

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

        [Command("gif")]
        [UsedImplicitly]
        public async Task Gif([Remainder] string searchValue)
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
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(string.Format(REDDIT_API_BASE_URL, searchValue));

                    var content = await response.Content.ReadAsStringAsync();

                    var deserialized = JsonConvert.DeserializeObject<RedditModel>(content);

                    if (deserialized == null || deserialized.Data.Posts.Length <= 0)
                    {
                        await ReplyAsync($"Couldn't find anything for {searchValue}");
                        return;
                    }

                    var randomIndex = Random.Next(0, deserialized.Data.Posts.Length - 1);

                    var videos = deserialized.Data.Posts[randomIndex];

                    await ReplyAsync(videos.Data.Url.ToString());
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed getting naughty video with search term {searchTerm}", searchValue);
            }
        }
    }
}