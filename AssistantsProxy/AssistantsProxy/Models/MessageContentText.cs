using System.Text.Json.Serialization;

namespace AssistantsProxy.Models
{
    public class MessageContentText
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("annotations")]
        public string[]? Annotations { get; set; }
    }
}
