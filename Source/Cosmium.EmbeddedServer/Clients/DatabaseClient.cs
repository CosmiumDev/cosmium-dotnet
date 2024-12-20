using System;
using System.Collections.Generic;
using System.Text.Json;
using Cosmium.EmbeddedServer.Interop;

namespace Cosmium.EmbeddedServer.Clients
{
    public class DatabaseClient
    {
        private readonly string instanceName;
        private readonly string databaseName;

        public DatabaseClient(string instanceName, string databaseName)
        {
            this.instanceName = instanceName;
            this.databaseName = databaseName;
        }

        public CollectionClient CreateCollection(string collectionName)
        {
            var collectionRequest = new Dictionary<string, string>
            {
                { "id", collectionName },
            };

            var requestJson = JsonSerializer.Serialize(collectionRequest);
            var result = CosmiumInterop.CreateCollection(instanceName, databaseName, requestJson);
            if (result != 0)
            {
                throw new Exception("Failed to create collection");
            }
            
            return new CollectionClient(instanceName, databaseName, collectionName);
        }

        public CollectionClient GetCollection(string collectionName)
        {
            return new CollectionClient(instanceName, databaseName, collectionName);
        }

        public bool DeleteCollection(string collectionName)
        {
            var result = CosmiumInterop.DeleteCollection(instanceName, databaseName, collectionName);
            return result == 0;
        }
    }
}
