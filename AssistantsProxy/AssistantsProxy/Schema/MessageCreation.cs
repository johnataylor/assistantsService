using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class MessageCreation
    {
        [JsonPropertyName("message_id")]
        public string? MessageId { get; set; }
    }
}
