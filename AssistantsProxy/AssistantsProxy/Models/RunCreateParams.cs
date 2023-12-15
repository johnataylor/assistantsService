using System.Text.Json.Serialization;

namespace AssistantsProxy.Models
{
    public class RunCreateParams
    {
        [JsonPropertyName("assistant_id")]
        public string? AsdsistantId { get; set; }
    }
}
