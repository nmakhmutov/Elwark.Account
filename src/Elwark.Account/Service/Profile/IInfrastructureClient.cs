using System.Threading.Tasks;
using Elwark.Account.Service.Profile.Models;

namespace Elwark.Account.Service.Profile
{
    public interface IInfrastructureClient
    {
        Task<ApiResponse<Country[]>> GetCountriesAsync();

        Task<ApiResponse<Timezone[]>> GetTimezonesAsync();
    }
}
