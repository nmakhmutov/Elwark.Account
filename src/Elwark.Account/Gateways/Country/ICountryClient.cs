using System.Threading.Tasks;

namespace Elwark.Account.Gateways.Country;

public interface ICountryClient
{
    Task<ApiResponse<Country[]>> GetAsync();
}