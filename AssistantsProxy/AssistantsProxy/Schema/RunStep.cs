using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class RunStep
    {
        // 'assistant.run.step'
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("assistant_id")]
        public string? AssistantId { get; set; }

        [JsonPropertyName("expires_at")]
        public long? ExpiresAt { get; set; }

        [JsonPropertyName("cancelled_at")]
        public long? CancelledAt { get; set; }

        [JsonPropertyName("failed_at")]
        public long? FailedAt { get; set; }

        [JsonPropertyName("completed_at")]
        public long? CompletedAt { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("last_error")]
        public LastError? LastError { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }

        [JsonPropertyName("run_id")]
        public string? RunId { get; set; }

        // 'in_progress' | 'cancelled' | 'failed' | 'completed' | 'expired';
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("step_details")]
        public StepDetailsBase? StepDetails { get; set; }

        [JsonPropertyName("thread_id")]
        public string? ThreadId { get; set; }

        // 'message_creation' | 'tool_calls'
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
