using Elwark.Account.Gateways.Profile.Models;
using Elwark.Account.Gateways.Profile.Requests;

namespace Elwark.Account.Gateways.Profile;

public interface IProfileClient
{
    Task<ApiResponse<Models.Profile>> GetAsync();
        
    Task<ApiResponse<Models.Profile>> UpdateAsync(UpdateProfileRequest profile);
        
    Task<ApiResponse<Profile.Models.Profile>> DeleteAsync(IdentityType type, string value);
        
    Task<ApiResponse<Confirming>> SendConfirmationAsync(IdentityType type, string value);

    Task<ApiResponse<Models.Profile>> ConfirmAsync(IdentityType type, string value, ConfirmRequest request);

    Task<ApiResponse<Models.Profile>> SetAsPrimaryAsync(string email);
        
    Task<ApiResponse<Confirming>> CreatingPasswordAsync();
        
    Task<ApiResponse<Models.Profile>> CreatePasswordAsync(CreatePasswordRequest request);
        
    Task<ApiResponse<bool>> UpdatePasswordAsync(UpdatePasswordRequest request);
}
