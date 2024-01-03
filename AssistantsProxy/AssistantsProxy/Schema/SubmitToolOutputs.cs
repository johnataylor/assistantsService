using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class SubmitToolOutputs
    {
        
        [JsonPropertyName("tool_calls")]
        public RequiredActionFunctionToolCall[]? ToolCalls { get; set; }
    }
}
