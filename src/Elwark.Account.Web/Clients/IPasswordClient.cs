using System.Threading.Tasks;
using Elwark.Account.Shared;
using Elwark.Account.Shared.Password;

namespace Elwark.Account.Web.Clients
{
    public interface IPasswordClient
    {
        Task<ApiResponse<bool>> IsAvailableAsync();
        
        Task<ApiResponse> RequestConfirmationAsync();
        
        Task<ApiResponse> CreateAsync(CreatePasswordModel model);
        
        Task<ApiResponse> UpdateAsync(UpdatePasswordModel model);
    }
}