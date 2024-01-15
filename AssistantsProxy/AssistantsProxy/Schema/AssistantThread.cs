using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class AssistantThread
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        // 'thread'
        [JsonPropertyName("object")]
        public required string Object { get; set; }

        [JsonPropertyName("created_at")]
        public required long CreatedAt { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
