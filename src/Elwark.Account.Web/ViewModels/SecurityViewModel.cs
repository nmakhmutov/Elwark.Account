using System.Threading.Tasks;
using Elwark.Account.Shared.PasswordService;
using Sotsera.Blazor.Toaster;

namespace Elwark.Account.Web.ViewModels
{
    public interface ISecurityViewModel
    {
        bool? IsPasswordAvailable { get; }
        
        Task LoadAsync();
        
        void PasswordAdded();
    }

    public class SecurityViewModel : ISecurityViewModel
    {
        private readonly IPasswordService _passwordService;
        private readonly IToaster _toaster;
        
        public SecurityViewModel(IPasswordService passwordService, IToaster toaster)
        {
            _passwordService = passwordService;
            _toaster = toaster;
        }

        public bool? IsPasswordAvailable { get; private set; }

        public void PasswordAdded() => IsPasswordAvailable = true;

        public async Task LoadAsync()
        {
            var result = await _passwordService.IsAvailableAsync();

            if (result.IsSuccess)
                IsPasswordAvailable = result.Data;
            else
                _toaster.Error(result.Error?.Detail);
        }
    }
}