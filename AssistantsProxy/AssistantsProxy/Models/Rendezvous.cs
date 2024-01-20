using System.Text.Json.Serialization;

namespace AssistantsProxy.Models
{
    public class Rendezvous
    {
        [JsonPropertyName("items")]
        public required RendezvousItem[] Items { get; set; }
    }
}
