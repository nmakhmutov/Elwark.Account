using System;
using System.Threading.Tasks;
using Elwark.Account.Shared.AccountService.Model;

namespace Elwark.Account.Shared.AccountService
{
    public interface IAccountService
    {
        Task<ApiResponse<AccountModel>> GetAsync();

        Task<ApiResponse<AccountModel>> UpdateAsync(AccountModel account);
        
        Task<ApiResponse> UpdatePictureAsync(Uri? picture);
    }
}