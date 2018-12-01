using Newtonsoft.Json;

namespace Disbot.Models.Reddit
{
    public class RedditChildModel
    {
        [JsonProperty("data")]
        public RedditChildDataModel Data { get; set; }
    }
}