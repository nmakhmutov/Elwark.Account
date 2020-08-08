using System;
using System.Threading.Tasks;
using Elwark.Account.Shared.PasswordService;
using Elwark.Account.Shared.PasswordService.Model;
using Sotsera.Blazor.Toaster;

namespace Elwark.Account.Web.ViewModels
{
    public interface ICreatePasswordViewModel
    {
        event Action OnChanged;
        CreatePasswordModel Model { get; }
        bool IsLoading { get; }
        bool IsCodeSending { get; }
        Task SendCodeAsync();
        Task CreateAsync();
    }

    public class CreatePasswordViewModel : ICreatePasswordViewModel
    {
        private readonly IPasswordService _passwordService;
        private readonly IToaster _toaster;

        public CreatePasswordViewModel(IPasswordService passwordService, IToaster toaster)
        {
            _passwordService = passwordService;
            _toaster = toaster;
        }

        public event Action OnChanged;

        public CreatePasswordModel Model { get; set; } = new CreatePasswordModel();

        public bool IsLoading { get; set; }

        public bool IsCodeSending { get; set; }

        public async Task SendCodeAsync()
        {
            IsCodeSending = true;

            var result = await _passwordService.SendCodeAsync();

            IsCodeSending = false;

            if (result.IsSuccess)
                _toaster.Success("Code sent");
            else
                _toaster.Error(result.Error?.Detail);
        }

        public async Task CreateAsync()
        {
            IsLoading = true;
            var result = await _passwordService.CreateAsync(Model);

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