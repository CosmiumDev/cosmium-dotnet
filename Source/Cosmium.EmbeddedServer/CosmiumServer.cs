using System;
using System.Runtime.InteropServices;
using Cosmium.EmbeddedServer.Contracts;
using Cosmium.EmbeddedServer.Interop;
using System.Text.Json;

namespace Cosmium.EmbeddedServer
{
    public static class CosmiumServer
    {
        public static int CreateInstance(string name, ServerConfiguration configuration)
        {
            var configurationJson = JsonSerializer.Serialize(configuration);

            return CosmiumInterop.CreateServerInstance(name, configurationJson);
        }

        public static int StopInstance(string serverName)
        {
            return CosmiumInterop.StopServerInstance(serverName);
        }

        public static ServerState? GetInstanceState(string serverName)
        {
            var resultPtr = CosmiumInterop.GetServerInstanceState(serverName);
            if (resultPtr == IntPtr.Zero)
            {
                return null;
            }

            var stateJson = Marshal.PtrToStringAnsi(resultPtr);
            return string.IsNullOrEmpty(stateJson) ? null : JsonSerializer.Deserialize<ServerState>(stateJson);
        }
    }
}
