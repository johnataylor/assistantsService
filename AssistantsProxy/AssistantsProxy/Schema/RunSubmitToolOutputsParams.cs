using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class RunSubmitToolOutputsParams
    {
        [JsonPropertyName("tool_outputs")]
        public ToolOutput[]? ToolOutputs { get; set; }
    }
}
