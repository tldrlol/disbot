using Newtonsoft.Json;

namespace Disbot.Models.Redtube
{
    public class NaughtyModel
    {
        [JsonProperty("videos")]
        public NaughtyMetaModel[] Videos { get; set; }
    }
}