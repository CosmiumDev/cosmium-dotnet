using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;
using Cosmium.EmbeddedServer.Interop;

namespace Cosmium.EmbeddedServer.Clients
{
    public class CollectionClient
    {
        private readonly string instanceName;
        private readonly string databaseName;
        private readonly string collectionName;

        public CollectionClient(string instanceName, string databaseName, string collectionName)
        {
            this.instanceName = instanceName;
            this.databaseName = databaseName;
            this.collectionName = collectionName;
        }

        public IEnumerable<T>? GetAll<T>() where T : class
        {
            var resultPtr = CosmiumInterop.GetAllDocuments(instanceName, databaseName, collectionName);
            if (resultPtr == IntPtr.Zero)
            {
                return null;
            }
            
            var resultStr = Marshal.PtrToStringAnsi(resultPtr);
            if (string.IsNullOrEmpty(resultStr))
            {
                return null;
            }

            return JsonSerializer.Deserialize<List<T>>(resultStr);
        }

        public T? GetById<T>(string id) where T : class
        {
            var resultPtr = CosmiumInterop.GetDocument(instanceName, databaseName, collectionName, id);
            if (resultPtr == IntPtr.Zero)
            {
                return null;
            }
            
            var resultStr = Marshal.PtrToStringAnsi(resultPtr);
            if (string.IsNullOrEmpty(resultStr))
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(resultStr);
        }

        public bool UpdateDocument<T>(string id, T document) where T : class
        {
            var documentStr = JsonSerializer.Serialize(document);
            var result = CosmiumInterop.UpdateDocument(instanceName, databaseName, collectionName, id, documentStr);
            return result == 0;
        }

        public bool CreateDocument<T>(string id, T document) where T : class
        {
            var documentStr = JsonSerializer.Serialize(document);
            var result = CosmiumInterop.CreateDocument(instanceName, databaseName, collectionName, documentStr);
            return result == 0;
        }

        public bool DeleteDocument(string id)
        {
            var result = CosmiumInterop.DeleteDocument(instanceName, databaseName, collectionName, id);
            return result == 0;
        }
    }
}