using System.Text.Json.Serialization;

namespace AssistantsProxy.Models
{
    public class Tool
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
