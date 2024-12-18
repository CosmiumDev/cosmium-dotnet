using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cosmium.EmbeddedServer.Contracts
{
    public class CollectionPartitionKeyState
    {
        [JsonPropertyName("paths")]
        public List<string> Paths { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("Version")]
        public int Version { get; set; }
    }
}