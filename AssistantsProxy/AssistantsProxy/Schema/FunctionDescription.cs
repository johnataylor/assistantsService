using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class FunctionDescription
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("parameters")]
        public JsonObject? Parameters { get; set; }
    }
}
