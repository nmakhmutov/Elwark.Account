using System.Collections.Generic;
using System.Threading.Tasks;
using Elwark.Account.Shared;
using Elwark.Account.Shared.Identity;
using Elwark.People.Abstractions;

namespace Elwark.Account.Web.Clients
{
    public interface IIdentityClient
    {
        Task<ApiResponse<IReadOnlyCollection<IdentityModel>>> GetAsync();
        Task<ApiResponse> AddAsync(Identification.Email email);
        Task<ApiResponse> DeleteAsync(IdentityId id);
        Task<ApiResponse> RequestConfirmationAsync(IdentityId id);
        Task<ApiResponse> ConfirmAsync(IdentityId id, long code);
        Task<ApiResponse> ChangeNotificationTypeAsync(IdentityId id, NotificationType type);
    }
}