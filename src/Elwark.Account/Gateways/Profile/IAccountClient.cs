using Elwark.Account.Gateways.Profile.Models;
using Elwark.Account.Gateways.Profile.Requests;

namespace Elwark.Account.Gateways.Profile;

public interface IAccountClient
{
    Task<ApiResponse<Models.Account>> GetAsync();
    
    Task<ApiResponse<Models.Account>> UpdateAsync(UpdateProfileRequest profile);
    
    Task<ApiResponse<Models.Account>> ConfirmAsync(ConfirmConnectionRequest request);
    
    Task<ApiResponse<Models.Account>> ChangePrimaryEmailAsync(UpdatePrimaryEmailRequest request);
    
    Task<ApiResponse<Models.Account>> DeleteAsync(IdentityType type, string value);
    
    Task<ApiResponse<Models.Account>> CreatePasswordAsync(CreatePasswordRequest request);
    
    Task<ApiResponse<bool>> UpdatePasswordAsync(UpdatePasswordRequest request);
    
    Task<ApiResponse<Confirming>> SendConfirmationAsync(CreateConfirmationRequest? request = null);
}
