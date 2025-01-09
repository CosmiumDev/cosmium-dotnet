using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cosmium.EmbeddedServer.JsonConverters
{
    internal class LowercaseEnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumString = reader.GetString();
            if (Enum.TryParse(enumString, ignoreCase: true, out T value))
            {
                return value;
            }
            throw new JsonException($"Unable to convert \"{enumString}\" to Enum \"{typeof(T)}\".");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLower());
        }
    }
}
