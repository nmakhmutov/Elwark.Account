using System.Net.Http;
using System.Threading.Tasks;
using Elwark.Account.Service.Profile.Models;

namespace Elwark.Account.Service.Profile
{
    public class InfrastructureClient : GatewayBase, IInfrastructureClient
    {
        private readonly HttpClient _client;

        public InfrastructureClient(HttpClient client) =>
            _client = client;

        public Task<ApiResponse<Country[]>> GetCountriesAsync() =>
            ExecuteAsync<Country[]>(() => _client.GetAsync("countries"));

        public Task<ApiResponse<Timezone[]>> GetTimezonesAsync() =>
            ExecuteAsync<Timezone[]>(() => _client.GetAsync("timezones"));
    }
}
