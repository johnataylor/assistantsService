using System.Text.Json.Serialization;

namespace AssistantsProxy.Models
{
    public class MessageCreateParams
    {
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }
}
