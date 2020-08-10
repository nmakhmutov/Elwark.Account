using System;
using System.Threading.Tasks;
using Elwark.Account.Shared.Account;
using Elwark.Account.Web.Clients;
using Sotsera.Blazor.Toaster;

namespace Elwark.Account.Web.State
{
    public class AccountState
    {
        private readonly IAccountClient _client;
        private readonly IToaster _toaster;
        private readonly AccountStore _store;

        public AccountState(IAccountClient client, IToaster toaster, AccountStore store)
        {
            _client = client;
            _toaster = toaster;
            _store = store;
        }

        public AccountModel Account
        {
            get => _store.Account;
            set => _store.Account = value;
        }

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
                _toaster.Error(result.Error?.Detail);
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
                _toaster.Success("Account updated");
                NotifyStateChanged();
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
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
                _toaster.Success("Picture updated");
                NotifyStateChanged();
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }
        }

        private void NotifyStateChanged() =>
            OnChange?.Invoke();
    }
}