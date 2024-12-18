using System.Text.Json.Serialization;

namespace Cosmium.EmbeddedServer.Contracts
{
    public class CollectionIndexState
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("dataType")]
        public string DataType { get; set; }

        [JsonPropertyName("precision")]
        public int Precision { get; set; }
    }
}