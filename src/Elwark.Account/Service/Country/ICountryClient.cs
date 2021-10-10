using System.Threading.Tasks;

namespace Elwark.Account.Service.Country
{
    public interface ICountryClient
    {
        Task<ApiResponse<Country[]>> GetAsync();
    }
}
