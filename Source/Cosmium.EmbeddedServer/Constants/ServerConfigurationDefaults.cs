using Cosmium.EmbeddedServer.Contracts;

namespace Cosmium.EmbeddedServer.Constants
{
    internal static class ServerConfigurationDefaults
    {
        internal const string AccountKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        internal const string ExplorerPath = "";
        internal const int Port = 8081;
        internal const string Host = "localhost";
        internal const string TlsCertificatePath = "";
        internal const string TlsCertificateKey = "";
        internal const string InitialDataFilePath = "";
        internal const string PersistDataFilePath = "";
        internal const bool DisableAuth = false;
        internal const bool DisableTls = false;
        internal static readonly LogLevel LogLevel = LogLevel.Silent;
    }
}