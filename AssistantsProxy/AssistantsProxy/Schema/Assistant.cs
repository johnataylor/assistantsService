using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class Assistant
    {
        [JsonPropertyName("id")]
        public required string Id { get; init; }

        // 'assistant'
        [JsonPropertyName("object")]
        public required string Object { get; init; }

        [JsonPropertyName("created_at")]
        public required long CreatedAt { get; init; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("instructions")]
        public string? Instructions { get; set; }

        [JsonPropertyName("tools")]
        public AssistantToolsBase[]? Tools { get; set; }

        [JsonPropertyName("file_ids")]
        public string[]? FileIds { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}

