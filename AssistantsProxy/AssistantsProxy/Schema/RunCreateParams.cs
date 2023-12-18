using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class RunCreateParams
    {
        [JsonPropertyName("assistant_id")]
        public string? AsdsistantId { get; set; }
    }
}
