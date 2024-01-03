using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class ToolOutput
    {
        [JsonPropertyName("output")]
        public string? Output { get; set; }

        [JsonPropertyName("tool_call_id")]
        public string? ToolCallId { get; set; }
    }
}
