using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class Function
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("arguments")]
        public string? Arguments { get; set; }
    }
}
