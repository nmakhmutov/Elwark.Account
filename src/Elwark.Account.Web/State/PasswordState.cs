using System.Threading.Tasks;
using Elwark.Account.Shared.Password;
using Elwark.Account.Web.Clients;
using Sotsera.Blazor.Toaster;

namespace Elwark.Account.Web.State
{
    public class PasswordState
    {
        private readonly IPasswordClient _client;
        private readonly IToaster _toaster;

        public PasswordState(IPasswordClient client, IToaster toaster)
        {
            _client = client;
            _toaster = toaster;
        }

        public bool? IsPasswordAvailable { get; private set; }

        public bool IsLoading { get; set; }

        public bool IsCodeSending { get; set; }

        public UpdatePasswordModel UpdateModel { get; set; } = new UpdatePasswordModel();

        public CreatePasswordModel CreateModel { get; set; } = new CreatePasswordModel();

        public async Task InitializeAsync()
        {
            var result = await _client.IsAvailableAsync();

            if (result.IsSuccess)
                IsPasswordAvailable = result.Data;
            else
                _toaster.Error(result.Error?.Detail);
        }

        public async Task RequestConfirmationAsync()
        {
            IsCodeSending = true;

            var result = await _client.RequestConfirmationAsync();

            IsCodeSending = false;

            if (result.IsSuccess)
                _toaster.Success("Code sent");
            else
                _toaster.Error(result.Error?.Detail);
        }

        public async Task CreateAsync()
        {
            IsLoading = true;
            var result = await _client.CreateAsync(CreateModel);

            IsLoading = false;

            if (result.IsSuccess)
            {
                IsPasswordAvailable = true;
                _toaster.Success("Password updated");
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }
        }

        public async Task UpdateAsync()
        {
            IsLoading = true;

            var result = await _client.UpdateAsync(UpdateModel);

            UpdateModel = new UpdatePasswordModel();
            
            IsLoading = false;

            if (result.IsSuccess)
            {
                _toaster.Success("Password updated");
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }
        }
    }
}