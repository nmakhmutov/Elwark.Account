using System;
using System.Threading.Tasks;
using Elwark.Account.Web.Services.AccountService.Model;

namespace Elwark.Account.Web.Services.AccountService
{
    public interface IAccountService
    {
        Task<ApiResponse<AccountModel>> GetAsync();

        Task<ApiResponse<AccountModel>> UpdateAsync(AccountModel account);
        
        Task<ApiResponse> UpdatePictureAsync(Uri? picture);
    }
}