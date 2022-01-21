namespace Elwark.Account.Gateways.Timezone;

internal sealed class TimezoneClient : GatewayBase, ITimezoneClient
{
    private readonly HttpClient _client;

    public TimezoneClient(HttpClient client) =>
        _client = client;

    public Task<ApiResponse<Timezone[]>> GetAsync() =>
        ExecuteAsync<Timezone[]>(ct => _client.GetAsync("timezones", ct));
}
