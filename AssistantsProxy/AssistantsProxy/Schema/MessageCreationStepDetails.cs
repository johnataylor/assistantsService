using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class MessageCreationStepDetails : StepDetailsBase
    {
        [JsonPropertyName("message_creation")]
        public MessageCreation? MessageCreation { get; set; }
    }
}
