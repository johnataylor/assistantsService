using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class RunCreateParams
    {
        [JsonPropertyName("assistant_id")]
        public string? AssistantId { get; set; }

        [JsonPropertyName("instructions")]
        public string? Instructions { get; set; }

        [JsonPropertyName("tools")]
        public Tool[]? Tools { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }
    }
}
