using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class RunUpdateParams
    {
        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
