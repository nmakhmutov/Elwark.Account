using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Elwark.Account.Gateways.Profile.Models;

namespace Elwark.Account.Gateways.Converters;

public class IdentityJsonConverter : JsonConverter<Connection?>
{
    private const string Type = "identityType";

    public override Connection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = JsonNode.Parse(ref reader);
        
        if (Enum.TryParse<IdentityType>(node?[Type]?.GetValue<string>(), true, out var type))
            return type switch
            {
                IdentityType.Email => node.Deserialize<EmailConnection>(options),
                IdentityType.Google => node.Deserialize<SocialConnection>(options),
                IdentityType.Microsoft => node.Deserialize<SocialConnection>(options),
                _ => throw new ArgumentOutOfRangeException()
            };

        return null;
    }

    public override void Write(Utf8JsonWriter writer, Connection? value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value, options);
}
