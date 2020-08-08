using System.Collections.Generic;
using System.Threading.Tasks;
using Elwark.Account.Shared.IdentityService.Model;
using Elwark.People.Abstractions;

namespace Elwark.Account.Shared.IdentityService
{
    public interface IIdentityService
    {
        Task<ApiResponse<IReadOnlyCollection<IdentityModel>>> GetAsync();
        Task<ApiResponse> AddAsync(AddIdentityModel model);
        Task<ApiResponse> DeleteAsync(IdentityId id);
        Task<ApiResponse> SendCodeAsync(IdentityId id);
        Task<ApiResponse> ConfirmAsync(ConfirmIdentityModel model);
        Task<ApiResponse> ChangeNotificationTypeAsync(ChangeNotificationTypeModel model);
    }
}