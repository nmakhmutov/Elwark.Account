using System;
using System.Threading.Tasks;
using Elwark.Account.Shared.Account;
using Elwark.Account.Web.Clients;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Toaster;

namespace Elwark.Account.Web.State
{
    public class AccountState
    {
        private readonly IAccountClient _client;
        private readonly IServiceProvider _provider;

        public AccountState(IAccountClient client, IServiceProvider provider)
        {
            _client = client;
            _provider = provider;
        }

        public AccountModel Account { get; set; }

        public bool IsLoading { get; set; }

        public event Action OnChange;

        public async Task InitializeAsync()
        {
            IsLoading = true;
            var result = await _client.GetAsync();
            IsLoading = false;

            if (result.IsSuccess)
            {
                Account = result.Data;
                NotifyStateChanged();
            }
            else
            {
                SendError(result.Error?.Detail);
            }
        }

        public async Task UpdateAsync()
        {
            IsLoading = true;
            var result = await _client.UpdateAsync(Account);
            IsLoading = false;

            if (result.IsSuccess)
            {
                Account = result.Data;
                NotifyStateChanged();
            }
            else
            {
                SendError(result.Error?.Detail);
            }
        }

        public async Task UpdatePictureAsync(Uri picture)
        {
            IsLoading = true;
            var result = await _client.UpdatePictureAsync(picture);
            IsLoading = false;

            if (result.IsSuccess)
            {
                Account.Picture = picture.ToString();
                NotifyStateChanged();
            }
            else
            {
                SendError(result.Error?.Detail);
            }
        }

        private void NotifyStateChanged() =>
            OnChange?.Invoke();

        private void SendError(string error)
        {
            using var scope = _provider.CreateScope();

            scope.ServiceProvider.GetService<IToaster>()
                .Error(error);
        }
    }
}