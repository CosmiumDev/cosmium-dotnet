using System.Text.Json.Serialization;

namespace Cosmium.EmbeddedServer.Contracts
{
    public class DatabaseState
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("_ts")]
        public long Timestamp { get; set; }

        [JsonPropertyName("_rid")]
        public string ResourceId { get; set; }

        [JsonPropertyName("_etag")]
        public string ETag { get; set; }

        [JsonPropertyName("_self")]
        public string Self { get; set; }
    }
}