using System.Text.Json.Serialization;

namespace AssistantsProxy.Schema
{
    public class LastError
    {
        // 'server_error' | 'rate_limit_exceeded'
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
