using System.Text.Json.Serialization;
using Cosmium.EmbeddedServer.Constants;

namespace Cosmium.EmbeddedServer.Contracts
{
    public class ServerConfiguration
    {
        [JsonPropertyName("AccountKey")]
        public string AccountKey { get; set; } = ServerConfigurationDefaults.AccountKey;

        [JsonPropertyName("ExplorerPath")]
        public string ExplorerPath { get; set; } = ServerConfigurationDefaults.ExplorerPath;

        [JsonPropertyName("Port")]
        public int Port { get; set; } = ServerConfigurationDefaults.Port;

        [JsonPropertyName("Host")]
        public string Host { get; set; } = ServerConfigurationDefaults.Host;

        [JsonPropertyName("TLS_CertificatePath")]
        public string TlsCertificatePath { get; set; } = ServerConfigurationDefaults.TlsCertificatePath;

        [JsonPropertyName("TLS_CertificateKey")]
        public string TlsCertificateKey { get; set; } = ServerConfigurationDefaults.TlsCertificateKey;

        [JsonPropertyName("InitialDataFilePath")]
        public string InitialDataFilePath { get; set; } = ServerConfigurationDefaults.InitialDataFilePath;

        [JsonPropertyName("PersistDataFilePath")]
        public string PersistDataFilePath { get; set; } = ServerConfigurationDefaults.PersistDataFilePath;

        [JsonPropertyName("DisableAuth")]
        public bool DisableAuth { get; set; } = ServerConfigurationDefaults.DisableAuth;

        [JsonPropertyName("DisableTls")]
        public bool DisableTls { get; set; } = ServerConfigurationDefaults.DisableTls;

        [JsonPropertyName("Debug")]
        public bool Debug { get; set; } = ServerConfigurationDefaults.Debug;
    }
}
