using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Elwark.Account.Shared.IdentityService.Model;
using Elwark.People.Abstractions;
using Newtonsoft.Json;

namespace Elwark.Account.Shared.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient) =>
            _httpClient = httpClient;

        public async Task<ApiResponse<IReadOnlyCollection<IdentityModel>>> GetAsync()
        {
            var data = await _httpClient.GetAsync("accounts/me/identities");

            return await data.GetResultAsync<IReadOnlyCollection<IdentityModel>>();
        }

        public async Task<ApiResponse> AddAsync(AddIdentityModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model.EmailIdentification), Encoding.UTF8, "application/json");
            var data = await _httpClient.PostAsync("accounts/me/attach/email", content);

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> SendCodeAsync(IdentityId id)
        {
            var data = await _httpClient.PostAsync($"accounts/me/identities/{id}/confirm",
                new StringContent(string.Empty));

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> ConfirmAsync(ConfirmIdentityModel model)
        {
            var data = await _httpClient.PutAsync($"accounts/me/identities/{model.Id}/confirm/{model.Code}",
                new StringContent(string.Empty));

            return await data.GetResultAsync();
        }

        public async Task<ApiResponse> ChangeNotificationTypeAsync(ChangeNotificationTypeModel model)
        {
            var data = await _httpClient.PutAsync($"accounts/me/identities/{model.Id}/notification/{model.Type}",
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