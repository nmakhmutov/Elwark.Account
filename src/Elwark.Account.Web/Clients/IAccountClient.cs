using System;
using System.Threading.Tasks;
using Elwark.Account.Shared;
using Elwark.Account.Shared.Account;

namespace Elwark.Account.Web.Clients
{
    public interface IAccountClient
    {
        Task<ApiResponse<AccountModel>> GetAsync();

        Task<ApiResponse<AccountModel>> UpdateAsync(AccountModel account);
        
        Task<ApiResponse> UpdatePictureAsync(Uri picture);
    }
}