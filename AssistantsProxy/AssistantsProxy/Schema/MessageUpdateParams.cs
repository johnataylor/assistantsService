using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class MessageUpdateParams
    {
        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
