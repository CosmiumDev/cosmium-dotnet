using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cosmium.EmbeddedServer.Contracts
{
    public class CollectionIndexingPolicyPathState
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("indexes")]
        public List<CollectionIndexState> Indexes { get; set; }
    }
}