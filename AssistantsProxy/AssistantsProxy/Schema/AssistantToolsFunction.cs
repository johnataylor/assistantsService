using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class AssistantToolsFunction : AssistantToolsBase
    {

        [JsonPropertyName("function")]
        public FunctionDescription? Function { get; set; }
    }
}
