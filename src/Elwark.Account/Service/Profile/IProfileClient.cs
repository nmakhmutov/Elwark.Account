using System.Threading.Tasks;
using Elwark.Account.Service.Profile.Models;
using Elwark.Account.Service.Profile.Requests;

namespace Elwark.Account.Service.Profile
{
    public interface IProfileClient
    {
        Task<ApiResponse<Models.Profile>> GetAsync();
        
        Task<ApiResponse<Models.Profile>> UpdateAsync(UpdateProfile profile);
        
        Task<ApiResponse<Profile.Models.Profile>> DeleteConnectionAsync(IdentityType type, string value);
        
        Task<ApiResponse<Confirming>> ConfirmingConnectionAsync(IdentityType type, string value);

        Task<ApiResponse<Models.Profile>> ConfirmConnectionAsync(IdentityType type, string value, Confirm request);

        Task<ApiResponse<Models.Profile>> ChangeEmailType(ChangeEmailType request);
        
        Task<ApiResponse<Confirming>> CreatingPasswordAsync();
        
        Task<ApiResponse<Models.Profile>> CreatePasswordAsync(CreatePasswordRequest request);
        
        Task<ApiResponse<string>> UpdatePasswordAsync(UpdatePasswordRequest request);
    }
}
