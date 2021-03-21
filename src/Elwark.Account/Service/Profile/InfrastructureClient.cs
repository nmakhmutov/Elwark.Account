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

        public Task<ApiResponse<Lists>> GetAsync() =>
            ExecuteAsync<Lists>(() => _client.GetAsync("infrastructure"));
    }
}