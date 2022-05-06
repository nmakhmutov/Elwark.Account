using Elwark.Account.Gateways.Profile.Models;
using Elwark.Account.Gateways.Profile.Requests;

namespace Elwark.Account.Gateways.Profile;

internal sealed class AccountClient : GatewayBase, IAccountClient
{
    private readonly HttpClient _client;

    public AccountClient(HttpClient client) =>
        _client = client;

    public Task<ApiResponse<Models.Account>> GetAsync() =>
        ExecuteAsync<Models.Account>(ct => _client.GetAsync("accounts/me", ct));

    public Task<ApiResponse<Models.Account>> UpdateAsync(UpdateProfileRequest profile) =>
        ExecuteAsync<Models.Account>(ct => _client.PutAsync("accounts/me", CreateJson(profile), ct));

    public Task<ApiResponse<Models.Account>> ConfirmAsync(ConfirmConnectionRequest request) =>
        ExecuteAsync<Models.Account>(ct => _client.PutAsync("accounts/me/connections", CreateJson(request), ct));

    public Task<ApiResponse<Models.Account>> ChangePrimaryEmailAsync(UpdatePrimaryEmailRequest request) =>
        ExecuteAsync<Models.Account>(ct => _client.PutAsync($"me/connections/primary-email", CreateJson(request), ct));

    public Task<ApiResponse<Models.Account>> DeleteAsync(IdentityType type, string value) =>
        ExecuteAsync<Models.Account>(ct => _client.DeleteAsync($"accounts/me/connections/{type}/{value}", ct));

    public Task<ApiResponse<Models.Account>> CreatePasswordAsync(CreatePasswordRequest request) =>
        ExecuteAsync<Models.Account>(ct => _client.PostAsync("accounts/me/password", CreateJson(request), ct));

    public Task<ApiResponse<bool>> UpdatePasswordAsync(UpdatePasswordRequest request) =>
        ExecuteAsync<bool>(ct => _client.PutAsync("accounts/me/password", CreateJson(request), ct));
    
    public Task<ApiResponse<Confirming>> SendConfirmationAsync(CreateConfirmationRequest? request)
    {
        var body = request is null ? null : CreateJson(request);
        return ExecuteAsync<Confirming>(ct => _client.PostAsync("accounts/me/confirmations", body, ct));
    }
}
