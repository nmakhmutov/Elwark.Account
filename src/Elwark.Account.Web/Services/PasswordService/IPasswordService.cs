using System.Threading.Tasks;
using Elwark.Account.Web.Services.PasswordService.Model;

namespace Elwark.Account.Web.Services.PasswordService
{
    public interface IPasswordService
    {
        Task<ApiResponse<bool>> IsAvailableAsync();
        Task<ApiResponse> SendCodeAsync();
        Task<ApiResponse> CreateAsync(CreatePasswordModel model);
        Task<ApiResponse> UpdateAsync(UpdatePasswordModel model);
    }
}