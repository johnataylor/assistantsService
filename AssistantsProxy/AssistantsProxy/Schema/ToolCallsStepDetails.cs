using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class ToolCallsStepDetails : StepDetailsBase
    {
        [JsonPropertyName("tool_calls")]
        public ToolCallBase[]? ToolCalls { get; set; }
    }
}
