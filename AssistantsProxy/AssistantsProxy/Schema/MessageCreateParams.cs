using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class MessageCreateParams
    {
        // 'user'
        [JsonPropertyName("role")]
        public required string Role { get; init; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("file_ids")]
        public string[]? FileIds { get; set; } = new string[0];

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
