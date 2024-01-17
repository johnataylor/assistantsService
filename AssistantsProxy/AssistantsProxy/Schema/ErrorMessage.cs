using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class ErrorMessage
    {
        [JsonPropertyName("error")]
        public required Error Error { get; init; }
    }
}
