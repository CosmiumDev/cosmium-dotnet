using System;
using System.Runtime.InteropServices;

namespace Cosmium.EmbeddedServer.Interop
{
    internal static class CosmiumInterop
    {
        private const string DllName = "cosmium";

        [DllImport(DllName, EntryPoint = "CreateServerInstance", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CreateServerInstance(string serverName, string configurationJson);

        [DllImport(DllName, EntryPoint = "StopServerInstance", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int StopServerInstance(string serverName);

        [DllImport(DllName, EntryPoint = "GetServerInstanceState", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetServerInstanceState(string serverName);
    }
}
