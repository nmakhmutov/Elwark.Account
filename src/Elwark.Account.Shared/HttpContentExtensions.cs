using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elwark.Account.Shared
{
    public static class HttpContentExtensions
    {
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer();

        public static async Task<ApiResponse<T>> GetResultAsync<T>(this HttpResponseMessage message)
        {
            var stream = await message.Content.ReadAsStreamAsync();

            using var sr = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(sr);

            return message.IsSuccessStatusCode
                ? ApiResponse<T>.Success(JsonSerializer.Deserialize<T>(jsonTextReader)!)
                : ApiResponse<T>.Fail(await ConvertAsync(jsonTextReader));
        }

        public static async Task<ApiResponse> GetResultAsync(this HttpResponseMessage message)
        {
            if (message.IsSuccessStatusCode)
                return ApiResponse.Success();

            var stream = await message.Content.ReadAsStreamAsync();

            using var sr = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(sr);

            return ApiResponse.Fail(await ConvertAsync(jsonTextReader));
        }

        private static async Task<ProblemDetails> ConvertAsync(JsonReader reader)
        {
            try
            {
                var json = await JObject.LoadAsync(reader);
                
                return json.ToObject<ProblemDetails>()!;
            }
            catch
            {
                return new ProblemDetails
                {
                    Title = "Unknown"
                };
            }
        }
    }
}