using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class CodeToolCallCodeInterpreter
    {
        [JsonPropertyName("input")]
        public string? Input { get; set; }

        // output
    }
}
