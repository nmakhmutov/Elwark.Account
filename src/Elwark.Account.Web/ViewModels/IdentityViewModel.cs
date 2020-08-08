using System;
using System.Threading.Tasks;
using Elwark.Account.Shared.IdentityService;
using Elwark.Account.Shared.IdentityService.Model;
using Elwark.People.Abstractions;
using Sotsera.Blazor.Toaster;

namespace Elwark.Account.Web.ViewModels
{
    public interface IIdentityViewModel
    {
        event Action OnChanged;
        IdentityId IdentityId { get; set; }
        Identification Identification { get; set; }
        Notification Notification { get; set; }
        DateTimeOffset? ConfirmedAt { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        bool IsLoading { get; set; }
        bool IsConfirmationCodeSent { get; set; }
        void Init(IdentityModel model);
        Task<bool> SendConfirmationCodeAsync();
        Task<bool> ConfirmIdentityAsync(long code);
        Task<bool> ChangeNotificationTypeAsync(NotificationType type);
        Task<bool> DeleteIdentityAsync();
    }
    
    public class IdentityViewModel : IIdentityViewModel
    {
        private readonly IIdentityService _identityService;
        private readonly IToaster _toaster;

        public IdentityViewModel(IIdentityService identityService, IToaster toaster)
        {
            _identityService = identityService;
            _toaster = toaster;
        }

        public event Action OnChanged;
        
        public IdentityId IdentityId { get; set; }

        public Identification Identification { get; set; }

        public Notification Notification { get; set; }

        public DateTimeOffset? ConfirmedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        
        public bool IsLoading { get; set; }
        
        public bool IsConfirmationCodeSent { get; set; }

        public void Init(IdentityModel model)
        {
            if (model is null) 
                throw new ArgumentNullException(nameof(model));
            
            IdentityId = model.IdentityId;
            Identification = model.Identification;
            Notification = model.Notification;
            ConfirmedAt = model.ConfirmedAt;
            CreatedAt = model.CreatedAt;
        }
        
        public async Task<bool> SendConfirmationCodeAsync()
        {
            IsLoading = true;
            var result = await _identityService.SendCodeAsync(IdentityId);
            IsLoading = false;
            
            if (result.IsSuccess)
            {
                _toaster.Success("Confirmation code sent");
                IsConfirmationCodeSent = true;
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }

            return result.IsSuccess;
        }

        public async Task<bool> ConfirmIdentityAsync(long code)
        {
            IsLoading = true;
            var result = await _identityService.ConfirmAsync(new ConfirmIdentityModel
            {
                Id = IdentityId,
                Code = code
            });
            IsLoading = false;
            
            if (result.IsSuccess)
            {
                NotifyStateChanged();
                _toaster.Success("Identity confirmed");
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }

            return result.IsSuccess;
        }

        public async Task<bool> ChangeNotificationTypeAsync(NotificationType type)
        {
            IsLoading = true;
            var result = await _identityService.ChangeNotificationTypeAsync(new ChangeNotificationTypeModel
            {
                Id = IdentityId,
                Type = type
            });
            IsLoading = false;
            
            if (result.IsSuccess)
            {
                NotifyStateChanged();
                _toaster.Success("Notification type changed");
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }

            return result.IsSuccess;
        }

        public async Task<bool> DeleteIdentityAsync()
        {
            IsLoading = true;
            var result = await _identityService.DeleteAsync(IdentityId);
            IsLoading = false;
            
            if (result.IsSuccess)
            {
                NotifyStateChanged();
                _toaster.Success("Identity removed");
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }

            return result.IsSuccess;
        }

        protected virtual void NotifyStateChanged()
        {
            OnChanged?.Invoke();
        }
    }
}