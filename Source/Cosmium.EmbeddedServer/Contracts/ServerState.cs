using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cosmium.EmbeddedServer.Contracts
{
    public class ServerState
    {
        [JsonPropertyName("databases")]
        public Dictionary<string, DatabaseState> Databases { get; set; }

        [JsonPropertyName("collections")]
        public Dictionary<string, Dictionary<string, CollectionState>> Collections { get; set; }

        [JsonPropertyName("documents")]
        public Dictionary<string, Dictionary<string, Dictionary<string, DocumentState>>> Documents { get; set; }
    }
}
