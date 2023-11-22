using System.Text.Json.Serialization;

namespace AssistantsProxy.Models
{
    public class RunRequest
    {
        [JsonPropertyName("assistant_id")]
        public string? AsdsistantId { get; set; }
    }
}
