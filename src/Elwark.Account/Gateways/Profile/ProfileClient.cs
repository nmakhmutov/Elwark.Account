using System.Net.Http;
using System.Threading.Tasks;
using Elwark.Account.Gateways.Profile.Models;
using Elwark.Account.Gateways.Profile.Requests;

namespace Elwark.Account.Gateways.Profile;

internal sealed class ProfileClient : GatewayBase, IProfileClient
{
    private readonly HttpClient _client;

    public ProfileClient(HttpClient client) =>
        _client = client;

    public Task<ApiResponse<Models.Profile>> GetAsync() =>
        ExecuteAsync<Models.Profile>(ct => _client.GetAsync("profiles/me", ct));

    public Task<ApiResponse<Models.Profile>> UpdateAsync(UpdateProfileRequest profile) =>
        ExecuteAsync<Models.Profile>(ct => _client.PutAsync("profiles/me", ToJson(profile), ct));

    public Task<ApiResponse<Confirming>> SendConfirmationAsync(IdentityType type, string value) =>
        ExecuteAsync<Confirming>(ct => _client.PostAsync($"profiles/me/connections/{type}/{value}/confirm", EmptyContent, ct));

    public Task<ApiResponse<Models.Profile>> ConfirmAsync(IdentityType type, string value, ConfirmRequest request) =>
        ExecuteAsync<Models.Profile>(ct => _client.PutAsync($"profiles/me/connections/{type}/{value}/confirm", ToJson(request), ct));

    public Task<ApiResponse<Models.Profile>> SetAsPrimaryAsync(string email) =>
        ExecuteAsync<Models.Profile>(ct => _client.PutAsync($"profiles/me/connections/email/{email}/primary", EmptyContent, ct));

    public Task<ApiResponse<Models.Profile>> DeleteAsync(IdentityType type, string value) =>
        ExecuteAsync<Models.Profile>(ct => _client.DeleteAsync($"profiles/me/connections/{type}/{value}", ct));

    public Task<ApiResponse<Confirming>> CreatingPasswordAsync() =>
        ExecuteAsync<Confirming>(ct => _client.PostAsync("profiles/me/password/confirm", EmptyContent, ct));

    public Task<ApiResponse<Models.Profile>> CreatePasswordAsync(CreatePasswordRequest request) =>
        ExecuteAsync<Models.Profile>(ct => _client.PostAsync("profiles/me/password", ToJson(request), ct));

    public Task<ApiResponse<bool>> UpdatePasswordAsync(UpdatePasswordRequest request) =>
        ExecuteAsync<bool>(ct => _client.PutAsync("profiles/me/password", ToJson(request), ct));
}
