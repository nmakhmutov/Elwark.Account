using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Elwark.Account.Shared.Identity;
using Elwark.Account.Web.Clients;
using Elwark.Account.Web.Models;
using Elwark.People.Abstractions;
using Microsoft.Extensions.Configuration;
using Sotsera.Blazor.Toaster;

namespace Elwark.Account.Web.State
{
    public class IdentitiesState
    {
        private const string ViewKey = "identities.view";
        private const string SortingKey = "identities.sorting";

        private readonly IIdentityClient _client;
        private readonly IToaster _toaster;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _configuration;

        private List<IdentityModel> _identities = new List<IdentityModel>();
        private IdentitySorting _sorting = IdentitySorting.None;
        private ViewType _view = ViewType.List;

        private event Action ViewChanged;
        private event Action SortingChanged;

        private static readonly IdentificationType[] Identifiers =
        {
            IdentificationType.Google,
            IdentificationType.Facebook,
            IdentificationType.Microsoft
        };

        public IdentitiesState(IIdentityClient client, IToaster toaster, ILocalStorageService localStorage,
            IConfiguration configuration)
        {
            _client = client;
            _toaster = toaster;
            _localStorage = localStorage;
            _configuration = configuration;

            ViewChanged += () => localStorage.SetItemAsync(ViewKey, View);
            SortingChanged += () =>
            {
                SortIdentities();
                localStorage.SetItemAsync(SortingKey, Sorting);
            };
        }

        public IReadOnlyCollection<IdentityModel> Identities => _identities.AsReadOnly();

        public IdentitySorting Sorting
        {
            get => _sorting;
            set
            {
                _sorting = value;
                OnSortingChanged();
            }
        }

        public ViewType View
        {
            get => _view;
            set
            {
                _view = value;
                OnViewChanged();
            }
        }

        public async Task InitializeAsync()
        {
            _view = await _localStorage.GetItemAsync<ViewType>(ViewKey);
            _sorting = await _localStorage.GetItemAsync<IdentitySorting>(SortingKey);

            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var data = await _client.GetAsync();

            if (data.IsSuccess)
            {
                _identities = data.Data.ToList();
                SortIdentities();
            }
            else
                _toaster.Error(data.Error?.Detail);
        }

        public async Task<bool> AddIdentityAsync(Identification.Email email)
        {
            var result = await _client.AddAsync(email);
            if (result.IsSuccess)
            {
                _toaster.Success("Email has been added");
                await LoadAsync();
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }

            return result.IsSuccess;
        }

        public IEnumerable<AttachLinkModel> GetAttachLinks(string returnUrl)
        {
            var host = new Uri(_configuration["Urls:IdentitySite"]);

            return Identifiers
                .Select(x => new AttachLinkModel(x, new Uri(host, $"/attach/provider/{x}?returnUrl={returnUrl}")));
        }

        public async Task<bool> SendConfirmationCodeAsync(IdentityModel model)
        {
            model.IsLoading = true;
            var result = await _client.RequestConfirmationAsync(model.IdentityId);
            model.IsLoading = false;

            if (result.IsSuccess)
            {
                _toaster.Success("Confirmation code sent");
                model.IsConfirmationCodeSent = true;
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }

            return result.IsSuccess;
        }

        public async Task<bool> ConfirmAsync(IdentityModel model, long code)
        {
            model.IsLoading = true;
            var result = await _client.ConfirmAsync(model.IdentityId, code);
            model.IsLoading = false;

            if (result.IsSuccess)
            {
                await LoadAsync();
                _toaster.Success("Identity confirmed");
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }

            return result.IsSuccess;
        }

        public async Task<bool> ChangeNotificationTypeAsync(IdentityModel model, NotificationType type)
        {
            model.IsLoading = true;
            var result = await _client.ChangeNotificationTypeAsync(model.IdentityId, type);
            model.IsLoading = false;

            if (result.IsSuccess)
            {
                await LoadAsync();
                _toaster.Success("Notification type changed");
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }

            return result.IsSuccess;
        }

        public async Task<bool> DeleteAsync(IdentityModel model)
        {
            model.IsLoading = true;
            var result = await _client.DeleteAsync(model.IdentityId);
            model.IsLoading = false;

            if (result.IsSuccess)
            {
                _identities.RemoveAll(x => x.IdentityId == model.IdentityId);
                _toaster.Success("Identity removed");
            }
            else
            {
                _toaster.Error(result.Error?.Detail);
            }

            return result.IsSuccess;
        }

        private void SortIdentities() =>
            _identities = Sorting switch
            {
                IdentitySorting.None => _identities.OrderBy(x => x.IdentityId).ToList(),

                IdentitySorting.TypeAsc => _identities.OrderBy(x => x.Identification.Type)
                    .ThenBy(x => x.IdentityId)
                    .ToList(),

                IdentitySorting.TypeDesc => _identities.OrderByDescending(x => x.Identification.Type)
                    .ThenBy(x => x.IdentityId)
                    .ToList(),

                IdentitySorting.AddedAsc => _identities.OrderBy(x => x.CreatedAt)
                    .ThenBy(x => x.IdentityId)
                    .ToList(),

                IdentitySorting.AddedDesc => _identities.OrderByDescending(x => x.CreatedAt)
                    .ThenBy(x => x.IdentityId)
                    .ToList(),

                _ => throw new ArgumentOutOfRangeException()
            };

        private void OnViewChanged() =>
            ViewChanged?.Invoke();

        private void OnSortingChanged()
        {
            SortingChanged?.Invoke();
        }
    }
}