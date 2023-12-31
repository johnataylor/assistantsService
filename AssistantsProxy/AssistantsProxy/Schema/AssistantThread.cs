﻿using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class AssistantThread
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        // 'thread'
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}
