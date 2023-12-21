using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class AssistantUpdateParams
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("file_ids")]
        public string[]? FileIds { get; set; }

        [JsonPropertyName("instructions")]
        public string? Instructions { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("tools")]
        public Tool[]? Tools { get; set; }
    }
}
