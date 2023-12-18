using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class MessageContentText
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("annotations")]
        public string[]? Annotations { get; set; }
    }
}
