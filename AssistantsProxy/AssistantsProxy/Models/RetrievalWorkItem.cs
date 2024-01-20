using AssistantsProxy.Schema;
using System.Text.Json.Serialization;

namespace AssistantsProxy.Models
{
    public class RetrievalWorkItem
    {
        [JsonPropertyName("thread_id")]
        public required string ThreadId { get; set; }

        [JsonPropertyName("run_id")]
        public required string RunId { get; set; }

        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("function")]
        public required RequiredActionFunctionToolCallFunction Function { get; set; }
    }
}
