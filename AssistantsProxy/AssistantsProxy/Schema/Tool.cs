using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class Tool
    {
        // code_interpreter | retrieval | function
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
