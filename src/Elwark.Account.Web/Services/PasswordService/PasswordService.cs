using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Elwark.Account.Web.Services.PasswordService.Model;
using Newtonsoft.Json;

namespace Elwark.Account.Web.Services.PasswordService
{
    public class PasswordService : IPasswordService
    {
        private readonly HttpClient _httpClient;

        public PasswordService(HttpClient httpClient) =>
            _httpClient = httpClient;

        public async Task<ApiResponse<bool>> IsAvailableAsync()
        {
            var result = await _httpClient.GetAsync("accounts/me/password");

            return await result.GetResultAsync<bool>();
        }

        public async Task<ApiResponse> SendCodeAsync()
        {
            var result = await _httpClient.PostAsync("accounts/me/password/code", new StringContent(string.Empty));

            return await result.GetResultAsync();
        }

        public async Task<ApiResponse> CreateAsync(CreatePasswordModel model)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(new {model.Code, model.Password}),
                Encoding.UTF8,
                "application/json"
            );

            var result = await _httpClient.PostAsync("accounts/me/password", content);

            return await result.GetResultAsync();
        }

        public async Task<ApiResponse> UpdateAsync(UpdatePasswordModel model)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(new {model.Current, model.Password}),
                Encoding.UTF8,
                "application/json"
            );

            var result = await _httpClient.PutAsync("accounts/me/password", content);

            return await result.GetResultAsync();
        }
    }
}