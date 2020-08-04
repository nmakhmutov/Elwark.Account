using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Elwark.People.Abstractions;
using Newtonsoft.Json;

namespace Elwark.Account.Web.Services.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient) =>
            _httpClient = httpClient;

        public async Task<ApiResponse<IReadOnlyCollection<Model.IdentityModel>>> GetAsync()
        {
            var data = await _httpClient.GetAsync("accounts/me/identities");

            return await data.GetResultAsync<IReadOnlyCollection<Model.IdentityModel>>();
        }

        public async Task<ApiResponse> AddAsync(Identification.Email email)
        {
            var content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
            var data = await _httpClient.PostAsync("accounts/me/attach/email", content);

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> SendCodeAsync(IdentityId id)
        {
            var data = await _httpClient.PostAsync($"accounts/me/identities/{id}/confirm",
                new StringContent(string.Empty));

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> ConfirmAsync(IdentityId id, long code)
        {
            var data = await _httpClient.PutAsync($"accounts/me/identities/{id}/confirm/{code}",
                new StringContent(string.Empty));

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> ChangeNotificationTypeAsync(IdentityId id, NotificationType type)
        {
            var data = await _httpClient.PutAsync($"accounts/me/identities/{id}/notification/{type}",
                new StringContent(string.Empty));

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> DeleteAsync(IdentityId id)
        {
            var data = await _httpClient.DeleteAsync($"accounts/me/identities/{id}");

            return await data.GetResultAsync();
        }
    }
}