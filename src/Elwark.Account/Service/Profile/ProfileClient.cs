using System.Net.Http;
using System.Threading.Tasks;
using Elwark.Account.Service.Profile.Models;
using Elwark.Account.Service.Profile.Requests;

namespace Elwark.Account.Service.Profile
{
    public class ProfileClient : GatewayBase, IProfileClient
    {
        private readonly HttpClient _client;

        public ProfileClient(HttpClient client) =>
            _client = client;

        public Task<ApiResponse<Models.Profile>> GetAsync() =>
            ExecuteAsync<Models.Profile>(() => _client.GetAsync("profiles/me"));

        public Task<ApiResponse<Models.Profile>> UpdateAsync(UpdateProfile profile) =>
            ExecuteAsync<Models.Profile>(() => _client.PutAsync("profiles/me", ToJson(profile)));

        public Task<ApiResponse<Confirming>> ConfirmingIdentityAsync(IdentityType type, string value) =>
            ExecuteAsync<Confirming>(() =>
                _client.PostAsync($"profiles/me/identities/{type}/value/{value}/confirm", EmptyContent));

        public Task<ApiResponse<Models.Profile>> ConfirmIdentityAsync(IdentityType type, string value,
            Confirm request) => ExecuteAsync<Models.Profile>(() =>
            _client.PutAsync($"profiles/me/identities/{type}/value/{value}/confirm", ToJson(request)));

        public Task<ApiResponse<Models.Profile>> ChangeEmailType(ChangeEmailType request) =>
            ExecuteAsync<Models.Profile>(() => _client.PutAsync("profiles/me/identities/email", ToJson(request)));

        public Task<ApiResponse<Models.Profile>> DeleteIdentityAsync(IdentityType type, string value) =>
            ExecuteAsync<Models.Profile>(() => _client.DeleteAsync($"profiles/me/identities/{type}/value/{value}"));
        
        public Task<ApiResponse<Confirming>> CreatingPasswordAsync() =>
            ExecuteAsync<Confirming>(() => _client.PostAsync("profiles/me/password/confirm", EmptyContent));
        
        public Task<ApiResponse<Models.Profile>> CreatePasswordAsync(CreatePasswordRequest request) =>
            ExecuteAsync<Models.Profile>(() => _client.PostAsync("profiles/me/password", ToJson(request)));

        public Task<ApiResponse<string>> UpdatePasswordAsync(UpdatePasswordRequest request) =>
            ExecuteAsync<string>(() => _client.PutAsync("profiles/me/password", ToJson(request)));
    }
}