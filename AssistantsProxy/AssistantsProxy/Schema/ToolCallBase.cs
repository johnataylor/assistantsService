using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(CodeToolCall), typeDiscriminator: "code_interpreter")]
    [JsonDerivedType(typeof(RetrievalToolCall), typeDiscriminator: "retrieval")]
    [JsonDerivedType(typeof(FunctionToolCall), typeDiscriminator: "function")]
    [SwaggerDiscriminator("type")]
    [SwaggerSubType(typeof(CodeToolCall), DiscriminatorValue = "code_interpreter")]
    [SwaggerSubType(typeof(RetrievalToolCall), DiscriminatorValue = "retrieval")]
    [SwaggerSubType(typeof(FunctionToolCall), DiscriminatorValue = "function")]

    public abstract class ToolCallBase
    {
    }
}
