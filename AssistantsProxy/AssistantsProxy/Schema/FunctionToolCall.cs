using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class FunctionToolCall : ToolCallBase
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("function")]
        public FunctionToolCallFunction? Function { get; set; }
    }
}
