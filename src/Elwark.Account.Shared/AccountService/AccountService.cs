using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Elwark.Account.Shared.AccountService.Model;
using Newtonsoft.Json;

namespace Elwark.Account.Shared.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient) =>
            _httpClient = httpClient;

        public async Task<ApiResponse<AccountModel>> GetAsync()
        {
            var data = await _httpClient.GetAsync("accounts/me");

            return await data.GetResultAsync<AccountModel>();
        }

        public async Task<ApiResponse<AccountModel>> UpdateAsync(AccountModel account)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(account),
                Encoding.UTF8,
                "application/json"
            );

            var result = await _httpClient.PutAsync("accounts/me", content);

            return await result.GetResultAsync<AccountModel>();
        }

        public async Task<ApiResponse> UpdatePictureAsync(Uri? picture)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(new {picture}),
                Encoding.UTF8,
                "application/json"
            );

            var result = await _httpClient.PutAsync("accounts/me/picture", content);

            return await result.GetResultAsync();
        }
    }
}