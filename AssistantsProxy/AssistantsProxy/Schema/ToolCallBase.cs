using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    [SwaggerDiscriminator("type")]
    [SwaggerSubType(typeof(CodeToolCall), DiscriminatorValue = "code_interpreter")]
    [SwaggerSubType(typeof(RetrievalToolCall), DiscriminatorValue = "retrieval")]
    [SwaggerSubType(typeof(FunctionToolCall), DiscriminatorValue = "function")]

    public abstract class ToolCallBase
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
