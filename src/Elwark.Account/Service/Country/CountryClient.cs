using System.Net.Http;
using System.Threading.Tasks;

namespace Elwark.Account.Service.Country;

internal sealed class CountryClient : GatewayBase,ICountryClient
{
    private readonly HttpClient _client;

    public CountryClient(HttpClient client) =>
        _client = client;

    public Task<ApiResponse<Country[]>> GetAsync() =>
        ExecuteAsync<Country[]>(() => _client.GetAsync("countries/all"));
}