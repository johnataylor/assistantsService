using AssistantsProxy.Schema;
using System.Text.Json.Serialization;

namespace AssistantsProxy.Services
{
    public class RunsWorkItemValue
    {
        // note the constructor parameter names are significant in the deserilization execution

        [JsonConstructor]
        public RunsWorkItemValue(string assistantId, string threadId, string runId, RunSubmitToolOutputsParams? runSubmitToolOutputsParams) =>
            (AssistantId, ThreadId, RunId, RunSubmitToolOutputsParams) = (assistantId, threadId, runId, runSubmitToolOutputsParams);

        [JsonPropertyName("assistant_id")]
        public string AssistantId { get; }

        [JsonPropertyName("thread_id")]
        public string ThreadId { get; }

        [JsonPropertyName("runs_id")]
        public string RunId { get; }

        [JsonPropertyName("tool_outputs")]
        public RunSubmitToolOutputsParams? RunSubmitToolOutputsParams { get; set; }
    }
}
