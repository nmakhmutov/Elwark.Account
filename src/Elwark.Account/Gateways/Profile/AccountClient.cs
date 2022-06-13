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

    public Task<ApiResponse<Email>> AddEmailAsync(EmailRequest email) =>
        ExecuteAsync<Email>(ct => _client.PostAsync("accounts/me/emails", CreateJson(email), ct));

    public Task<ApiResponse<bool>> DeleteEmailAsync(string email) =>
        ExecuteAsync<bool>(ct => _client.DeleteAsync($"accounts/me/emails/{email}", ct));

    public Task<ApiResponse<Email[]>> SetPrimaryEmailAsync(EmailRequest email) =>
        ExecuteAsync<Email[]>(ct => _client.PostAsync("accounts/me/emails/status", CreateJson(email), ct));

    public Task<ApiResponse<Confirming>> ConfirmingEmailAsync(EmailRequest email) =>
        ExecuteAsync<Confirming>(ct => _client.PostAsync("accounts/me/emails/verify", CreateJson(email), ct));

    public Task<ApiResponse<Email>> ConfirmEmailAsync(ConfirmRequest confirm) =>
        ExecuteAsync<Email>(ct => _client.PutAsync("accounts/me/emails/verify", CreateJson(confirm), ct));

    public Task<ApiResponse<bool>> DeleteConnectionAsync(ExternalService type, string identity)
    {
        var service = type switch
        {
            ExternalService.Google => "google",
            ExternalService.Microsoft => "microsoft",
            _ => "unknown"
        };

        var url = $"accounts/me/connections/{service}/identities/{identity}";
        return ExecuteAsync<bool>(ct => _client.DeleteAsync(url, ct));
    }
}
