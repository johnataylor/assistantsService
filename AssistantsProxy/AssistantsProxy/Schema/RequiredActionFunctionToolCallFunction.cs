using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class RequiredActionFunctionToolCallFunction
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("arguments")]
        public string? Arguments { get; set; }
    }
}
