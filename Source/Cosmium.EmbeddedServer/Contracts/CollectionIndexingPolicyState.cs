using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cosmium.EmbeddedServer.Contracts
{
    public class CollectionIndexingPolicyState
    {
        [JsonPropertyName("indexingMode")]
        public string IndexingMode { get; set; }

        [JsonPropertyName("automatic")]
        public bool Automatic { get; set; }

        [JsonPropertyName("includedPaths")]
        public List<CollectionIndexingPolicyPathState> IncludedPaths { get; set; }

        [JsonPropertyName("excludedPaths")]
        public List<CollectionIndexingPolicyPathState> ExcludedPaths { get; set; }
    }
}