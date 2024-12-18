using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cosmium.EmbeddedServer.Contracts
{
    public class DocumentState
    {
        [JsonExtensionData]
        public Dictionary<string, object> Properties { get; set; }
    }
}