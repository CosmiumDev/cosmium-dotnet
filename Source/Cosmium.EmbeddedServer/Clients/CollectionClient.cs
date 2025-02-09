using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cosmium.EmbeddedServer.Contracts;
using Cosmium.EmbeddedServer.Helpers;
using Cosmium.EmbeddedServer.Interop;

namespace Cosmium.EmbeddedServer.Clients
{
    internal class CollectionClient : ICollectionClient
    {
        private readonly string instanceName;
        private readonly string databaseName;
        private readonly string collectionName;
        private readonly IDocumentSerializer serializer;

        public CollectionClient(string instanceName, string databaseName, string collectionName, IDocumentSerializer serializer)
        {
            this.instanceName = instanceName;
            this.databaseName = databaseName;
            this.collectionName = collectionName;
            this.serializer = serializer;
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            var resultPtr = CosmiumInterop.GetAllDocuments(instanceName, databaseName, collectionName);
            if (resultPtr == IntPtr.Zero)
            {
                return null;
            }
            
            var resultStr = Marshal.PtrToStringAnsi(resultPtr);
            CosmiumInterop.FreeMemory(resultPtr);

            if (string.IsNullOrEmpty(resultStr))
            {
                return null;
            }
            
            return JsonSerializationHelper.FromJson<List<T>>(resultStr, serializer);
        }

        public T GetById<T>(string id) where T : class
        {
            var resultPtr = CosmiumInterop.GetDocument(instanceName, databaseName, collectionName, id);
            if (resultPtr == IntPtr.Zero)
            {
                return null;
            }
            
            var resultStr = Marshal.PtrToStringAnsi(resultPtr);
            CosmiumInterop.FreeMemory(resultPtr);

            if (string.IsNullOrEmpty(resultStr))
            {
                return null;
            }

            return JsonSerializationHelper.FromJson<T>(resultStr, serializer);
        }

        public bool UpdateDocument<T>(string id, T document) where T : class
        {
            var documentStr = JsonSerializationHelper.ToJson(document, serializer);
            var result = CosmiumInterop.UpdateDocument(instanceName, databaseName, collectionName, id, documentStr);
            return result == 0;
        }

        public bool CreateDocument<T>(string id, T document) where T : class
        {
            var documentStr = JsonSerializationHelper.ToJson(document, serializer);
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