using System;
using System.Threading.Tasks;
using Elwark.Account.Shared.PasswordService;
using Elwark.Account.Shared.PasswordService.Model;
using Sotsera.Blazor.Toaster;

namespace Elwark.Account.Web.ViewModels
{
    public interface IUpdatePasswordViewModel
    {
        UpdatePasswordModel Model { get; set; }

        bool IsLoading { get; set; }

        Task UpdateAsync();

        event Action OnChanged;
    }

    public class UpdatePasswordViewModel : IUpdatePasswordViewModel
    {
        private readonly IPasswordService _passwordService;
        private readonly IToaster _toaster;

        public UpdatePasswordViewModel(IPasswordService passwordService, IToaster toaster)
        {
            _passwordService = passwordService;
            _toaster = toaster;
        }

        public event Action OnChanged;

        public UpdatePasswordModel Model { get; set; } = new UpdatePasswordModel();

        public bool IsLoading { get; set; }

        public async Task UpdateAsync()
        {
            IsLoading = true;

            var result = await _passwordService.UpdateAsync(Model);

            IsLoading = false;

            if (result.IsSuccess)
            {
                _toaster.Success("Password updated");
                NotifyStateChanged();
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }
        }

        private void NotifyStateChanged()
        {
            OnChanged?.Invoke();
        }
    }
}