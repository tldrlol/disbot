using Newtonsoft.Json;

namespace Disbot.Models.Redtube
{
    public class NaughtyMetaModel
    {
        [JsonProperty("video")]
        public VideoModel Video { get; set; }
    }
}