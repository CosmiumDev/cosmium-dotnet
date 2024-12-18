using System.Text.Json.Serialization;

namespace Cosmium.EmbeddedServer.Contracts
{
    public class CollectionState
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("indexingPolicy")]
        public CollectionIndexingPolicyState IndexingPolicy { get; set; }

        [JsonPropertyName("partitionKey")]
        public CollectionPartitionKeyState PartitionKey { get; set; }

        [JsonPropertyName("_rid")]
        public string ResourceId { get; set; }

        [JsonPropertyName("_ts")]
        public long Timestamp { get; set; }

        [JsonPropertyName("_self")]
        public string Self { get; set; }

        [JsonPropertyName("_etag")]
        public string ETag { get; set; }

        [JsonPropertyName("_docs")]
        public string Docs { get; set; }

        [JsonPropertyName("_sprocs")]
        public string Sprocs { get; set; }

        [JsonPropertyName("_triggers")]
        public string Triggers { get; set; }

        [JsonPropertyName("_udfs")]
        public string Udfs { get; set; }

        [JsonPropertyName("_conflicts")]
        public string Conflicts { get; set; }
    }
}