using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class ThreadMessage
    {
        [JsonPropertyName("id")]
        public required string Id { get; init; }

        // 'thread.message'
        [JsonPropertyName("object")]
        public required string Object { get; init; }

        [JsonPropertyName("created_at")]
        public required long CreatedAt { get; init; }

        [JsonPropertyName("thread_id")]
        public required string ThreadId { get; init; }

        // 'user' | 'assistant'
        [JsonPropertyName("role")]
        public required string Role { get; init; }

        [JsonPropertyName("content")]
        public MessageContent[]? Content { get; set; }

        [JsonPropertyName("file_ids")]
        public string[]? FileIds { get; set; }

        [JsonPropertyName("assistant_id")]
        public string? AssistantId { get; set; }

        [JsonPropertyName("run_id")]
        public string? RunId { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
