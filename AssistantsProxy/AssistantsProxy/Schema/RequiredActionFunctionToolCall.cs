using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class RequiredActionFunctionToolCall
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        // 'function'
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("function")]
        public RequiredActionFunctionToolCallFunction? Function { get; set; }
    }
}
