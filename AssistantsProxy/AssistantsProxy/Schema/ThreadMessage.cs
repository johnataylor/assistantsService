﻿using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class ThreadMessage
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        // 'thread.message'
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        // 'user' | 'assistant'
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("content")]
        public MessageContent[]? Content { get; set; }

        [JsonPropertyName("file_ids")]
        public string[]? FileIds { get; set; }

        [JsonPropertyName("assistant_id")]
        public string? AssistantId { get; set; }

        [JsonPropertyName("run_id")]
        public string? RunId { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
