using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Elwark.Account.Gateways.Converters;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Elwark.Account.Gateways;

public abstract class GatewayBase
{
    private static readonly JsonSerializerOptions Serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            new IdentityJsonConverter()
        },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };

    protected static readonly StringContent EmptyContent = new(string.Empty, Encoding.UTF8, "application/json");

    protected static async Task<ApiResponse<T>> ExecuteAsync<T>(Func<Task<HttpResponseMessage>> handler)
    {
        try
        {
            using var message = await handler();
            await using var stream = await message.Content.ReadAsStreamAsync();
            
            return message.IsSuccessStatusCode
                ? ApiResponse<T>.Success(JsonSerializer.Deserialize<T>(stream, Serializer)!)
                : ApiResponse<T>.Fail(JsonSerializer.Deserialize<Error>(stream, Serializer)!);
        }
        catch (AccessTokenNotAvailableException ex)
        {
            ex.Redirect();
            return ApiResponse<T>.Fail(Error.Create("Unauthorized", "https://tools.ietf.org/html/rfc7235#section-3.1", 401));
        }
        catch (HttpRequestException)
        {
            return ApiResponse<T>.Fail(Error.Create("Unavailable", "https://tools.ietf.org/html/rfc7231#section-6.6.4", 503));
        }
        catch (Exception)
        {
            return ApiResponse<T>.Fail(Error.Create("Internal", "https://tools.ietf.org/html/rfc7231#section-6.6.3", 502));
        }
    }

    protected static StringContent ToJson<T>(T value) =>
        new(JsonSerializer.Serialize(value, Serializer), Encoding.UTF8, "application/json");
}
