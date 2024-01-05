using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class CodeToolCall : ToolCallBase
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("code_interpreter")]
        public CodeToolCallCodeInterpreter? CodeInterpreter {  get; set; }
    }
}
