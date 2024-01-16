using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class AssistantList<T>
    {
        // 'list'
        [JsonPropertyName("object")]
        public required string Object { get; init; }

        [JsonPropertyName("data")]
        public required T[] Data { get; init; }

        // TODO implement paging

        [JsonPropertyName("first_id")]
        public string? FirstId { get; set; }

        [JsonPropertyName("last_id")]
        public string? LastId { get; set; }

        [JsonPropertyName("has_more")]
        public bool? HasMore { get; set; }
    }
}
