using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class AssistantCreateParams
    {
        [JsonPropertyName("model")]
        public string? Model { get; init; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("file_ids")]
        public string[] FileIds { get; init; } = new string[0];

        [JsonPropertyName("instructions")]
        public string? Instructions { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("tools")]
        public AssistantToolsBase[] Tools { get; init; } = new AssistantToolsBase[0];
    }
}
