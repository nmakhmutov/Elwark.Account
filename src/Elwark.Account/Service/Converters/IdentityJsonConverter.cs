using System;
using Elwark.Account.Service.Profile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elwark.Account.Service.Converters
{
    public class IdentityJsonConverter : JsonConverter<Connection?>
    {
        private const string Type = nameof(Connection.IdentityType);

        public override void WriteJson(JsonWriter writer, Connection? value, JsonSerializer serializer) =>
            serializer.Serialize(writer, value);

        public override Connection? ReadJson(JsonReader reader, Type objectType, Connection? existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JObject jObject = JObject.Load(reader);
            if (jObject.Type == JTokenType.Null)
                return null;

            var type = jObject.GetValue(Type, StringComparison.InvariantCultureIgnoreCase)?.Value<string>() ?? "";

            return Enum.Parse<IdentityType>(type, true) switch
            {
                IdentityType.Email => jObject.ToObject<EmailConnection>(),
                IdentityType.Google => jObject.ToObject<SocialConnection>(),
                IdentityType.Microsoft => jObject.ToObject<SocialConnection>(),
                _ => throw new ArgumentOutOfRangeException(nameof(Connection), @"Unknown identity type")
            };
        }
    }
}