using System;
using Newtonsoft.Json;

namespace Disbot.Models.Reddit
{
    public class RedditChildDataModel
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}