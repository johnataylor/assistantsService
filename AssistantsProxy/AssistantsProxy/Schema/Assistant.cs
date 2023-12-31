﻿using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class Assistant
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        // 'assistant'
        [JsonPropertyName("object")]
        public string? Object { get; set; }

        [JsonPropertyName("created_at")]
        public long? CreatedAt { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("instructions")]
        public string? Instructions { get; set; }

        [JsonPropertyName("tools")]
        public AssistantToolsBase[]? Tools { get; set; }

        [JsonPropertyName("file_ids")]
        public string[]? FileIds { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }
    }
}

