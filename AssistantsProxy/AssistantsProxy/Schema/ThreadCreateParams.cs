using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class ThreadCreateParams
    {
        [JsonPropertyName("messages")]
        public MessageCreateParams[]? Messages { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
