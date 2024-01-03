using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class RequiredAction
    {
        [JsonPropertyName("submit_tool_outputs")]
        public SubmitToolOutputs? SubmitToolOutputs { get; set; }

        // 'submit_tool_outputs'
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
