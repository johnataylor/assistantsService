﻿using System.Text.Json.Serialization;

namespace AssistantsProxy.Models
{
    public class AssistantUpdateParams
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("instructions")]
        public string? Instructions { get; set; }

        [JsonPropertyName("tools")]
        public Tool[]? Tools { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }
    }
}
