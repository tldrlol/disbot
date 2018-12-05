using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Disbot.Extensions;
using Disbot.Models.Reddit;
using Discord;
using Discord.Commands;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Disbot.Modules
{
    [Group("reddit"), Alias("r")]
    [UsedImplicitly]
    public class RedditFeedModule : ModuleBase
    {
        private const string REDDIT_URI = "https://www.reddit.com/r/{0}/top/.json";
        private const int MAX_ITEMS_TO_TAKE = 5;
        private const string DEFAULT_SUBREDDIT = "politics";

        [Command]
        [UsedImplicitly]
        public Task GetFeed()
        {
            return GetFeed(DEFAULT_SUBREDDIT);
        }

        [Command]
        [UsedImplicitly]
        public async Task GetFeed(string subreddit)
        {
            // Hack to format users that want to specify the vanity for a
            // subreddit
            if (subreddit.StartsWith("/r/"))
            {
                subreddit = subreddit.Replace("/r/", string.Empty);
            }
            else if (subreddit.StartsWith("r/"))
            {
                subreddit = subreddit.Replace("r/", string.Empty);
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(string.Format(REDDIT_URI, subreddit));

                var content = await response.Content.ReadAsStringAsync();

                var deserialized = JsonConvert.DeserializeObject<RedditModel>(content);

                if (deserialized?.Data?.Posts == null || deserialized.Data.Posts.Any() == false)
                {
                    await ReplyAsync($"Couldn't find anything on /r/{subreddit} :(");
                    return;
                }

                foreach (var post in deserialized.Data.Posts.Where(x => x.Data.IsMeta == false).Take(MAX_ITEMS_TO_TAKE))
                {
                    var embed = new EmbedBuilder().AddInlineField(post.Data.Title.Truncate(256, true), post.Data.Url);

                    if (!string.IsNullOrWhiteSpace(post.Data.Thumbnail) && Uri.IsWellFormedUriString(post.Data.Thumbnail, UriKind.Absolute))
                        embed.WithThumbnailUrl(post.Data.Thumbnail);

                    embed.WithFooter($"Posted by {post.Data.Author} | {post.Data.Upvotes} 👍");

                    await ReplyAsync(string.Empty, embed: embed);
                }
            }
        }
    }
}
