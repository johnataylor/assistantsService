﻿using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class RequiredActionFunctionToolCall
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        // 'function'
        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("function")]
        public required RequiredActionFunctionToolCallFunction Function { get; set; }
    }
}
