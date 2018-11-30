using Newtonsoft.Json;

namespace Disbot.Models
{
    public class NaughtyMetaModel
    {
        [JsonProperty("video")]
        public VideoModel Video { get; set; }
    }
}