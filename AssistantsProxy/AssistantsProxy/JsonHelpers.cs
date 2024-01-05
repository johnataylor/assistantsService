using System.Text.Json;
using System.Text;

namespace AssistantsProxy
{
    public static class JsonHelpers
    {
        const string TypeDiscriminator = "type";

        /// <summary>
        /// JSON JsonPolymorphic deserialization works to instantiate concrete derived types.
        /// However, the type descriminator property is required to be the first property in the JSON object.
        /// Super lame bug. Refer to https://github.com/dotnet/runtime/issues/72604
        /// This code simply rewrite the JSON. More optimal approaches are possible.
        /// </summary>
        /// <param name="oldJson"></param>
        /// <returns>JSON with any "type" properties moved to the front of any objects</returns>
        public static string FixJson(string oldJson)
        {
            var doc = JsonDocument.Parse(oldJson);
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
            Walk(doc.RootElement, writer);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            var newJson = Encoding.UTF8.GetString(stream.ToArray());
            return newJson;
        }

        private static void Walk(JsonElement value, Utf8JsonWriter writer)
        {
            if (value.ValueKind == JsonValueKind.Object)
            {
                WalkObject(value, writer);
            }
            else if (value.ValueKind == JsonValueKind.Array)
            {
                WalkArray(value, writer);
            }
            else
            {
                writer.WriteRawValue(value.GetRawText());
            }
        }

        private static void WalkObject(JsonElement element, Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            var typeProperty = element.EnumerateObject().FirstOrDefault(p => p.NameEquals(TypeDiscriminator));
            if (typeProperty.Value.ValueKind != JsonValueKind.Undefined)
            {
                writer.WriteString(typeProperty.Name, typeProperty.Value.ToString());
            }
            foreach (var property in element.EnumerateObject())
            {
                if (!property.NameEquals(TypeDiscriminator))
                {
                    writer.WritePropertyName(property.Name);
                    Walk(property.Value, writer);
                }
            }
            writer.WriteEndObject();
        }

        private static void WalkArray(JsonElement element, Utf8JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var value in element.EnumerateArray())
            {
                Walk(value, writer);
            }
            writer.WriteEndArray();
        }
    }
}
