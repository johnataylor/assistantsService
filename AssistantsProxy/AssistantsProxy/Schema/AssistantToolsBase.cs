using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(AssistantToolsCode), typeDiscriminator: "code_interpreter")]
    [JsonDerivedType(typeof(AssistantToolsRetrieval), typeDiscriminator: "retrieval")]
    [JsonDerivedType(typeof(AssistantToolsFunction), typeDiscriminator: "function")]
    [SwaggerDiscriminator("type")]
    [SwaggerSubType(typeof(AssistantToolsCode), DiscriminatorValue = "code_interpreter")]
    [SwaggerSubType(typeof(AssistantToolsRetrieval), DiscriminatorValue = "retrieval")]
    [SwaggerSubType(typeof(AssistantToolsFunction), DiscriminatorValue = "function")]
    public abstract class AssistantToolsBase
    {
    }
}
