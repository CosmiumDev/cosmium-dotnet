using System;
using System.Collections.Generic;
using Cosmium.EmbeddedServer.Contracts;
using Cosmium.EmbeddedServer.Helpers;
using Cosmium.EmbeddedServer.Interop;

namespace Cosmium.EmbeddedServer.Clients
{
    internal class DatabaseClient : IDatabaseClient
    {
        private readonly string instanceName;
        private readonly string databaseName;
        private IDocumentSerializer serializer;

        public DatabaseClient(string instanceName, string databaseName, IDocumentSerializer serializer)
        {
            this.instanceName = instanceName;
            this.databaseName = databaseName;
            this.serializer = serializer;
        }

        public ICollectionClient CreateCollection(string collectionName)
        {
            var collectionRequest = new Dictionary<string, string>
            {
                { "id", collectionName },
            };

            var requestJson = JsonSerializationHelper.ToJson(collectionRequest);
            var result = CosmiumInterop.CreateCollection(instanceName, databaseName, requestJson);
            if (result != 0)
            {
                throw new Exception("Failed to create collection");
            }
            
            return new CollectionClient(instanceName, databaseName, collectionName, serializer);
        }

        public ICollectionClient GetCollection(string collectionName)
        {
            return new CollectionClient(instanceName, databaseName, collectionName, serializer);
        }

        public bool DeleteCollection(string collectionName)
        {
            var result = CosmiumInterop.DeleteCollection(instanceName, databaseName, collectionName);
            return result == 0;
        }
    }
}
