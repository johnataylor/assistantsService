using System.Text.Json.Serialization;

namespace AssistantsProxy.Services
{
    public class Notification
    {
        [JsonPropertyName("thread_id")]
        public required string ThreadId { get; init; }

        [JsonPropertyName("run_id")]
        public required string RunId { get; init; }

        [JsonPropertyName("status")]
        public required string Status { get; init; }
    }
}
