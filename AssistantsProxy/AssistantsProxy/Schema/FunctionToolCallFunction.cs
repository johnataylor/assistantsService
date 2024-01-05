using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class FunctionToolCallFunction
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("arguments")]
        public string? Arguments { get; set; }

        [JsonPropertyName("output")]
        public string? Output { get; set; }
    }
}
