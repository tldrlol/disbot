using System;
using Newtonsoft.Json;

namespace Disbot.Models.Reddit
{
    public class RedditChildDataModel
    {
        [JsonProperty("author_fullname")]
        public string AuthorFullname { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ups")]
        public long Upvotes { get; set; }

        [JsonProperty("is_meta")]
        public bool IsMeta { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("over_18")]
        public bool Over18 { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("num_comments")]
        public long NumComments { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}