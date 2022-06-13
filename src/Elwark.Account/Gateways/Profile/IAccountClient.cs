using Elwark.Account.Gateways.Profile.Models;
using Elwark.Account.Gateways.Profile.Requests;

namespace Elwark.Account.Gateways.Profile;

public interface IAccountClient
{
    Task<ApiResponse<Models.Account>> GetAsync();

    Task<ApiResponse<Models.Account>> UpdateAsync(UpdateProfileRequest profile);

    Task<ApiResponse<Email>> AddEmailAsync(EmailRequest email);

    Task<ApiResponse<bool>> DeleteEmailAsync(string email);

    Task<ApiResponse<Email[]>> SetPrimaryEmailAsync(EmailRequest email);

    Task<ApiResponse<Confirming>> ConfirmingEmailAsync(EmailRequest email);

    Task<ApiResponse<Email>> ConfirmEmailAsync(ConfirmRequest confirm);

    Task<ApiResponse<bool>> DeleteConnectionAsync(ExternalService type, string identity);
}
