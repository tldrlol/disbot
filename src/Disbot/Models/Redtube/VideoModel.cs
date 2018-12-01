using System;
using Newtonsoft.Json;

namespace Disbot.Models.Redtube
{
    public class VideoModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("embed_url")]
        public Uri EmbedUrl { get; set; }
    }
}