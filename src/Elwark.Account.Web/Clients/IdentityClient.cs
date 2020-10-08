using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Elwark.Account.Shared;
using Elwark.Account.Shared.Identity;
using Elwark.People.Abstractions;
using Newtonsoft.Json;

namespace Elwark.Account.Web.Clients
{
    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _httpClient;

        public IdentityClient(HttpClient httpClient) =>
            _httpClient = httpClient;

        public async Task<ApiResponse<IReadOnlyCollection<IdentityModel>>> GetAsync()
        {
            var data = await _httpClient.GetAsync("me/identities");

            return await data.GetResultAsync<IReadOnlyCollection<IdentityModel>>();
        }

        public async Task<ApiResponse> AddAsync(Identification.Email email)
        {
            var content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
            var data = await _httpClient.PostAsync("me/email", content);

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> RequestConfirmationAsync(IdentityId id)
        {
            var data = await _httpClient.PostAsync($"me/identities/{id}/confirm",
                new StringContent(string.Empty));

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> ConfirmAsync(IdentityId id, long code)
        {
            var data = await _httpClient.PutAsync($"me/identities/{id}/confirm/{code}",
                new StringContent(string.Empty));

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> ChangeNotificationTypeAsync(IdentityId id, NotificationType type)
        {
            var data = await _httpClient.PutAsync($"me/identities/{id}/notification/{type}",
                new StringContent(string.Empty));

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> DeleteAsync(IdentityId id)
        {
            var data = await _httpClient.DeleteAsync($"me/identities/{id}");

            return await data.GetResultAsync();
        }
    }
}