﻿using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class ThreadRun
    {
        [JsonPropertyName("id")]
        public required string Id { get; init; }

        // 'assistant.run'
        [JsonPropertyName("object")]
        public required string Object { get; init; }

        [JsonPropertyName("created_at")]
        public required long CreatedAt { get; init; }

        [JsonPropertyName("assistant_id")]
        public required string AssistantId { get; init; }

        [JsonPropertyName("thread_id")]
        public required string ThreadId { get; init; }

        // 'queued' | 'in_progress' | 'requires_action' | 'cancelling' | 'cancelled' | 'failed' | 'completed' | 'expired'
        [JsonPropertyName("status")]
        public required string Status { get; set; }

        [JsonPropertyName("started_at")]
        public long? StartedAt { get; set; }

        [JsonPropertyName("expires_at")]
        public long? ExpiresAt { get; set; }

        [JsonPropertyName("cancelled_at")]
        public long? CancelledAt { get; set; }

        [JsonPropertyName("failed_at")]
        public long? FailedAt { get; set; }

        [JsonPropertyName("completed_at")]
        public long? CompletedAt { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("instructions")]
        public string? Instructions { get; set; }

        [JsonPropertyName("required_action")]
        public RequiredAction? RequiredAction { get; set; }

        [JsonPropertyName("last_error")]
        public LastError? LastError { get; set; }

        [JsonPropertyName("tools")]
        public AssistantToolsBase[]? Tools { get; set; }

        [JsonPropertyName("file_ids")]
        public string[]? FileIds { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
