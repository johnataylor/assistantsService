﻿using System.Text.Json.Serialization;

namespace AssistantsProxy.Models
{
    public class MessageContent
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("text")]
        public MessageContentText? Text { get; set; }
    }
}
