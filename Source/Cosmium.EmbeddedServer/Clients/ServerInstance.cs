using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using Cosmium.EmbeddedServer.Contracts;
using Cosmium.EmbeddedServer.Interop;

namespace Cosmium.EmbeddedServer.Clients
{
    public class ServerInstance : IDisposable
    {
        private readonly string instanceName;
        private readonly ServerConfiguration serverConfiguration;

        public ServerInstance(string instanceName, ServerConfiguration serverConfiguration)
        {
            this.instanceName = instanceName;
            this.serverConfiguration = serverConfiguration;

            var configurationJson = JsonSerializer.Serialize(serverConfiguration);
            var createInstanceResult = CosmiumInterop.CreateServerInstance(instanceName, configurationJson);
            if (createInstanceResult != 0)
            {
                throw new Exception("Failed to create instance"); // TODO: Custom exceptions
            }
        }
        
        public string Endpoint => $"https://{serverConfiguration.Host}:{serverConfiguration.Port}/";
        public string AccountKey => serverConfiguration.AccountKey;

        public DatabaseClient CreateDatabase(string databaseName)
        {
            var result = CosmiumInterop.CreateDatabase(instanceName, databaseName);
            if (result != 0)
            {
                throw new Exception("Failed to create database");
            }

            return new DatabaseClient(instanceName, databaseName);
        }

        public DatabaseClient GetDatabase(string databaseName)
        {
            return new DatabaseClient(instanceName, databaseName);
        }

        public bool DeleteDatabase(string databaseName)
        {
            var result = CosmiumInterop.DeleteDatabase(instanceName, databaseName);
            return result == 0;
        }

        public ServerState? GetServerState()
        {
            var resultPtr = CosmiumInterop.GetServerInstanceState(instanceName);
            if (resultPtr == IntPtr.Zero)
            {
                return null;
            }

            var stateJson = Marshal.PtrToStringAnsi(resultPtr);
            if (string.IsNullOrEmpty(stateJson))
            {
                return null;
            }
            
            return JsonSerializer.Deserialize<ServerState>(stateJson);
        }

        public static bool LoadInstanceState(string serverName, ServerState state)
        {
            var stateJson = JsonSerializer.Serialize(state);

            var result = CosmiumInterop.LoadServerInstanceState(serverName, stateJson);
            return result == 0;
        }

        public void Dispose()
        {
            CosmiumInterop.StopServerInstance(instanceName);
        }
    }
}
