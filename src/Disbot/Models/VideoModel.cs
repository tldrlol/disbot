using Newtonsoft.Json;
using System;

namespace Disbot.Models
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