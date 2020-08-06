using System.Collections.Generic;
using System.Threading.Tasks;
using Elwark.People.Abstractions;

namespace Elwark.Account.Shared.IdentityService
{
    public interface IIdentityService
    {
        Task<ApiResponse<IReadOnlyCollection<Model.IdentityModel>>> GetAsync();
        Task<ApiResponse> AddAsync(Identification.Email email);
        Task<ApiResponse> DeleteAsync(IdentityId id);
        Task<ApiResponse> SendCodeAsync(IdentityId id);
        Task<ApiResponse> ConfirmAsync(IdentityId id, long code);
        Task<ApiResponse> ChangeNotificationTypeAsync(IdentityId id, NotificationType type);
    }
}