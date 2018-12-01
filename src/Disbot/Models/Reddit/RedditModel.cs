using Newtonsoft.Json;

namespace Disbot.Models.Reddit
{
    public class RedditModel
    {
        [JsonProperty("data")]
        public RedditDataModel Data { get; set; }
    }
}