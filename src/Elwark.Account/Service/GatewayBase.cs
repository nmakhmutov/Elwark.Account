using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Elwark.Account.Service.Converters;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Elwark.Account.Service
{
    public abstract class GatewayBase
    {
        private static readonly JsonSerializer Serializer = new()
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters =
            {
                new IsoDateTimeConverter(),
                new StringEnumConverter(new CamelCaseNamingStrategy()),
                new IdentityJsonConverter()
            }
        };
        
        protected static readonly StringContent EmptyContent = new(string.Empty, Encoding.UTF8, "application/json");
        
        protected static async Task<ApiResponse<T>> ExecuteAsync<T>(Func<Task<HttpResponseMessage>> handler)
        {
            try
            {
                using var message = await handler();
                await using var stream = await message.Content.ReadAsStreamAsync();
                using var sr = new StreamReader(stream);
                using var jsonTextReader = new JsonTextReader(sr);
                if (message.IsSuccessStatusCode)
                    return ApiResponse<T>.Success(Serializer.Deserialize<T>(jsonTextReader)!);

                var error = Serializer.Deserialize<Error>(jsonTextReader)
                            ?? Error.Unknown;

                return ApiResponse<T>.Fail(error);
            }
            catch (AccessTokenNotAvailableException ex)
            {
                ex.Redirect();
                return ApiResponse<T>.Fail(Error.Unauthorized);
            }
            catch (HttpRequestException)
            {
                return ApiResponse<T>.Fail(Error.Unavailable);
            }
            catch (Exception)
            {
                return ApiResponse<T>.Fail(Error.Unknown);
            }
        }

        protected static StringContent ToJson<T>(T value)
        {
            var sb = new StringBuilder(256);
            var sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using var jsonWriter = new JsonTextWriter(sw) {Formatting = Serializer.Formatting};

            Serializer.Serialize(jsonWriter, value);

            return new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
        }
    }
}
