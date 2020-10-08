using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Elwark.Account.Shared;
using Elwark.Account.Shared.Password;
using Newtonsoft.Json;

namespace Elwark.Account.Web.Clients
{
    public class PasswordClient : IPasswordClient
    {
        private readonly HttpClient _httpClient;

        public PasswordClient(HttpClient httpClient) =>
            _httpClient = httpClient;

        public async Task<ApiResponse<bool>> IsAvailableAsync()
        {
            var result = await _httpClient.GetAsync("me/password");

            return await result.GetResultAsync<bool>();
        }

        public async Task<ApiResponse> RequestConfirmationAsync()
        {
            var result = await _httpClient.PostAsync("me/password/code", new StringContent(string.Empty));

            return await result.GetResultAsync();
        }

        public async Task<ApiResponse> CreateAsync(CreatePasswordModel model)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(new {model.Code, model.Password}),
                Encoding.UTF8,
                "application/json"
            );

            var result = await _httpClient.PostAsync("me/password", content);

            return await result.GetResultAsync();
        }

        public async Task<ApiResponse> UpdateAsync(UpdatePasswordModel model)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(new {model.Current, model.Password}),
                Encoding.UTF8,
                "application/json"
            );

            var result = await _httpClient.PutAsync("me/password", content);

            return await result.GetResultAsync();
        }
    }
}