namespace Elwark.Account.Gateways.Country;

internal sealed class CountryClient : GatewayBase,ICountryClient
{
    private readonly HttpClient _client;

    public CountryClient(HttpClient client) =>
        _client = client;

    public Task<ApiResponse<Country[]>> GetAsync() =>
        ExecuteAsync<Country[]>(ct => _client.GetAsync("countries", ct));
}
