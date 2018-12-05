using Newtonsoft.Json;

namespace Disbot.Models.Reddit
{
    public class RedditDataModel
    {
        [JsonProperty("children")]
        public RedditChildModel[] Posts { get; set; }
    }
}