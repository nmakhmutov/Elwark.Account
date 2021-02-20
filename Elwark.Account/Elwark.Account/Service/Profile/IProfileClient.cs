using System.Threading.Tasks;
using Elwark.Account.Service.Profile.Models;

namespace Elwark.Account.Service.Profile
{
    public interface IProfileClient
    {
        Task<ApiResponse<Models.Profile>> GetAsync();
        
        Task<ApiResponse<Models.Profile>> UpdateAsync(UpdateProfile profile);
    }
}