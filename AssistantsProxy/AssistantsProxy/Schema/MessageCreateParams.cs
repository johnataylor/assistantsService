using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class MessageCreateParams
    {
        // 'user'
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("file_ids")]
        public string[]? FileIds { get; set; }

        [JsonPropertyName("metadata")]
        public string? Metadata { get; set; }
    }
}
