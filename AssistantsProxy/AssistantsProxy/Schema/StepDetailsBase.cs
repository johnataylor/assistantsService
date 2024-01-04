using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    // Refer to Swashbuckle docs
    // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#enrich-polymorphic-base-classes-with-discriminator-metadata

    //[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    //[JsonDerivedType(typeof(MessageCreationStepDetails), typeDiscriminator: "message_creation")]
    //[JsonDerivedType(typeof(ToolCallsStepDetails), typeDiscriminator: "tool_calls")]

    [SwaggerDiscriminator("type")]
    [SwaggerSubType(typeof(MessageCreationStepDetails), DiscriminatorValue = "message_creation")]
    [SwaggerSubType(typeof(ToolCallsStepDetails), DiscriminatorValue = "tool_calls")]
    public abstract class StepDetailsBase
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
