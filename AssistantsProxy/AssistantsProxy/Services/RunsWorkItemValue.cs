using System.Text.Json.Serialization;

namespace AssistantsProxy.Services
{
    public class RunsWorkItemValue
    {
        [JsonPropertyName("assistant_id")]
        public string? AssistantId { get; set; }

        [JsonPropertyName("thread_id")]
        public string? ThreadId { get; set; }

        [JsonPropertyName("runs_id")]
        public string? RunId { get; set; }
    }
}
