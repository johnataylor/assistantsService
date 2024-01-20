using AssistantsProxy.Schema;
using System.Text.Json.Serialization;

namespace AssistantsProxy.Models
{
    public class RendezvousItem
    {
        [JsonPropertyName("server_tool_call_function")]
        public RequiredActionFunctionToolCallFunction? ServerToolCallFunction { get; set; }

        [JsonPropertyName("tool_call_id")]
        public required string ToolCallId { get; set; }

        [JsonPropertyName("output")]
        public string? Output { get; set; }
    }
}
