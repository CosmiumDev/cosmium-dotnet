#if NET7_0_OR_GREATER
using System;
using System.Runtime.InteropServices;

namespace Cosmium.EmbeddedServer.Interop
{
    internal static partial class CosmiumInterop
    {
        private const string DllName = "cosmium";

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial long CreateCollection(string serverName, string databaseId, string collectionJson);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial IntPtr GetCollection(string serverName, string databaseId, string collectionId);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial IntPtr GetAllCollections(string serverName, string databaseId);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial long DeleteCollection(string serverName, string databaseId, string collectionId);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial long CreateDatabase(string serverName, string databaseJson);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial IntPtr GetDatabase(string serverName, string databaseId);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial IntPtr GetAllDatabases(string serverName);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial long DeleteDatabase(string serverName, string databaseId);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial long CreateDocument(string serverName, string databaseId, string collectionId, string documentJson);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial IntPtr GetDocument(string serverName, string databaseId, string collectionId, string documentId);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial IntPtr GetAllDocuments(string serverName, string databaseId, string collectionId);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial long UpdateDocument(string serverName, string databaseId, string collectionId, string documentId, string documentJson);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial long DeleteDocument(string serverName, string databaseId, string collectionId, string documentId);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial long CreateServerInstance(string serverName, string configurationJson);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial long StopServerInstance(string serverName);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial IntPtr GetServerInstanceState(string serverName);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial long LoadServerInstanceState(string serverName, string stateJson);

        [LibraryImport(DllName, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial void FreeMemory(IntPtr ptr);
    }
}
#endif
