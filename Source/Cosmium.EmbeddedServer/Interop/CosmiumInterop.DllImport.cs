#if !NET7_0_OR_GREATER
using System;
using System.Runtime.InteropServices;

namespace Cosmium.EmbeddedServer.Interop
{
    internal static class CosmiumInterop
    {
        private const string DllName = "cosmium";

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long CreateCollection(string serverName, string databaseId, string collectionJson);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetCollection(string serverName, string databaseId, string collectionId);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetAllCollections(string serverName, string databaseId);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long DeleteCollection(string serverName, string databaseId, string collectionId);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long CreateDatabase(string serverName, string databaseJson);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetDatabase(string serverName, string databaseId);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetAllDatabases(string serverName);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long DeleteDatabase(string serverName, string databaseId);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long CreateDocument(string serverName, string databaseId, string collectionId, string documentJson);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetDocument(string serverName, string databaseId, string collectionId, string documentId);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetAllDocuments(string serverName, string databaseId, string collectionId);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long UpdateDocument(string serverName, string databaseId, string collectionId, string documentId, string documentJson);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long DeleteDocument(string serverName, string databaseId, string collectionId, string documentId);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long CreateServerInstance(string serverName, string configurationJson);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long StopServerInstance(string serverName);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetServerInstanceState(string serverName);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long LoadServerInstanceState(string serverName, string stateJson);

        [DllImport(DllName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void FreeMemory(IntPtr ptr);
    }
}
#endif
