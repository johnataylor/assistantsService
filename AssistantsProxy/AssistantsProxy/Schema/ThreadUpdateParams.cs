using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class ThreadUpdateParams
    {
        [JsonPropertyName("messages")]
        public MessageCreateParams[]? Messages { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
