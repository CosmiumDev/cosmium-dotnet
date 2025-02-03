using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cosmium.EmbeddedServer.Contracts;
using Cosmium.EmbeddedServer.Helpers;
using Cosmium.EmbeddedServer.Interop;

namespace Cosmium.EmbeddedServer.Clients
{
    public class ServerInstance : IDisposable
    {
        private readonly string instanceName;
        private readonly ServerConfiguration serverConfiguration;
        private readonly IDocumentSerializer? serializer;

        public ServerInstance(string instanceName, ServerConfiguration serverConfiguration, IDocumentSerializer? serializer = null)
        {
            this.instanceName = instanceName;
            this.serverConfiguration = serverConfiguration;
            this.serializer = serializer;

            var configurationJson = JsonSerializationHelper.ToJson(serverConfiguration);
            var createInstanceResult = CosmiumInterop.CreateServerInstance(instanceName, configurationJson);
            if (createInstanceResult != 0)
            {
                throw new Exception("Failed to create instance"); // TODO: Custom exceptions
            }
        }
        
        public string Endpoint => $"https://{serverConfiguration.Host}:{serverConfiguration.Port}/";
        public string AccountKey => serverConfiguration.AccountKey;

        public IDatabaseClient CreateDatabase(string databaseName)
        {
            var databaseRequest = new Dictionary<string, string>()
            {
                { "id", databaseName },
            };

            var requestJson = JsonSerializationHelper.ToJson(databaseRequest);
            var result = CosmiumInterop.CreateDatabase(instanceName, requestJson);
            if (result != 0)
            {
                throw new Exception("Failed to create database");
            }

            return new DatabaseClient(instanceName, databaseName, serializer);
        }

        public IDatabaseClient GetDatabase(string databaseName)
        {
            return new DatabaseClient(instanceName, databaseName, serializer);
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
            CosmiumInterop.FreeMemory(resultPtr);

            if (string.IsNullOrEmpty(stateJson))
            {
                return null;
            }
            
            return JsonSerializationHelper.FromJson<ServerState>(stateJson);
        }

        public static bool LoadInstanceState(string serverName, ServerState state)
        {
            var stateJson = JsonSerializationHelper.ToJson(state);

            var result = CosmiumInterop.LoadServerInstanceState(serverName, stateJson);
            return result == 0;
        }

        public void Dispose()
        {
            CosmiumInterop.StopServerInstance(instanceName);
        }
    }
}
