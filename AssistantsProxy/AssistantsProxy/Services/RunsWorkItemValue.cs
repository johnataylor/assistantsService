using System.Text.Json.Serialization;

namespace AssistantsProxy.Services
{
    public class RunsWorkItemValue
    {
        [JsonConstructor]
        public RunsWorkItemValue(string assistantId, string threadId, string runId) =>
            (AssistantId, ThreadId, RunId) = (assistantId, threadId, runId);

        [JsonPropertyName("assistant_id")]
        public string AssistantId { get; }

        [JsonPropertyName("thread_id")]
        public string ThreadId { get; }

        [JsonPropertyName("runs_id")]
        public string RunId { get; }
    }
}
