using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class RequiredActionFunctionToolCallFunction
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("arguments")]
        public required string Arguments { get; set; }
    }
}
