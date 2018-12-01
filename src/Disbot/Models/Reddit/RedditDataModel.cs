using Newtonsoft.Json;

namespace Disbot.Models.Reddit
{
    public class RedditDataModel
    {
        [JsonProperty("children")]
        public RedditChildModel[] Children { get; set; }
    }
}