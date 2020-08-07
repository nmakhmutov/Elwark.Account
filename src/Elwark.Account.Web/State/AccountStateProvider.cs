using System;
using Elwark.Account.Shared.AccountService.Model;

namespace Elwark.Account.Web.State
{
    public class AccountStateProvider
    {
        public AccountModel Account { get; set; }
        
        public event Action OnChange;

        public void Update(AccountModel account)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            
            NotifyStateChanged();
        }
        
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}