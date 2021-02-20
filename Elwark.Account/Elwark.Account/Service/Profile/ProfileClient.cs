using System.Net.Http;
using System.Threading.Tasks;
using Elwark.Account.Service.Profile.Models;

namespace Elwark.Account.Service.Profile
{
    public class ProfileClient : GatewayBase, IProfileClient
    {
        private readonly HttpClient _client;

        public ProfileClient(HttpClient client) =>
            _client = client;

        public Task<ApiResponse<Models.Profile>> GetAsync() =>
            ExecuteAsync<Models.Profile>(() => _client.GetAsync("profiles"));

        public Task<ApiResponse<Models.Profile>> UpdateAsync(UpdateProfile profile) =>
            ExecuteAsync<Models.Profile>(() => _client.PutAsync("profiles", ToJson(profile)));
    }
}