using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class AssistantThread
    {
        [JsonPropertyName("id")]
        public required string Id { get; init; }

        // 'thread'
        [JsonPropertyName("object")]
        public required string Object { get; init; }

        [JsonPropertyName("created_at")]
        public required long CreatedAt { get; init; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
