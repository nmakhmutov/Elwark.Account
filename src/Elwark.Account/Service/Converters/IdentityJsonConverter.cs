using System;
using Elwark.Account.Service.Profile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elwark.Account.Service.Converters
{
    public class IdentityJsonConverter : JsonConverter<Identity?>
    {
        private const string Type = nameof(Identity.IdentityType);

        public override void WriteJson(JsonWriter writer, Identity? value, JsonSerializer serializer) =>
            serializer.Serialize(writer, value);

        public override Identity? ReadJson(JsonReader reader, Type objectType, Identity? existingValue,
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
                IdentityType.Email => jObject.ToObject<EmailIdentity>(),
                IdentityType.Google => jObject.ToObject<SocialIdentity>(),
                IdentityType.Microsoft => jObject.ToObject<SocialIdentity>(),
                _ => throw new ArgumentOutOfRangeException(nameof(Identity), @"Unknown identity type")
            };
        }
    }
}