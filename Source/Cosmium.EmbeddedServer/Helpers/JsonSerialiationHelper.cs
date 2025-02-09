using System.IO;
using System.Text;
using System.Text.Json;
using Cosmium.EmbeddedServer.Contracts;

namespace Cosmium.EmbeddedServer.Helpers
{
    internal static class JsonSerializationHelper
    {
        internal static string ToJson<T>(T input, IDocumentSerializer serializer = null)
        {
            if (serializer == null)
            {
                return JsonSerializer.Serialize(input);
            }

            using var stream = serializer.ToStream<T>(input);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        internal static T FromJson<T>(string input, IDocumentSerializer serializer = null)
        {
            if (serializer == null)
            {
                return JsonSerializer.Deserialize<T>(input);
            }

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            return serializer.FromStream<T>(stream);
        }
    }
}