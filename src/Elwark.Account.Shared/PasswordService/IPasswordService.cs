using System.Threading.Tasks;
using Elwark.Account.Shared.PasswordService.Model;

namespace Elwark.Account.Shared.PasswordService
{
    public interface IPasswordService
    {
        Task<ApiResponse<bool>> IsAvailableAsync();
        Task<ApiResponse> SendCodeAsync();
        Task<ApiResponse> CreateAsync(CreatePasswordModel model);
        Task<ApiResponse> UpdateAsync(UpdatePasswordModel model);
    }
}