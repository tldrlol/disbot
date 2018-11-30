using Newtonsoft.Json;

namespace Disbot.Models
{
    public class NaughtyModel
    {
        [JsonProperty("videos")]
        public NaughtyMetaModel[] Videos { get; set; }
    }
}