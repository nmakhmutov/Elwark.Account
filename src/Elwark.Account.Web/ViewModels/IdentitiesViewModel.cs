using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Elwark.Account.Shared.IdentityService;
using Elwark.Account.Shared.IdentityService.Model;
using Elwark.Account.Web.Models;
using Elwark.People.Abstractions;
using Microsoft.Extensions.Configuration;
using Sotsera.Blazor.Toaster;

namespace Elwark.Account.Web.ViewModels
{
    public interface IIdentitiesViewModel
    {
        IReadOnlyCollection<IdentityModel> Identities { get; }

        IdentitySorting Sorting { get; set; }
        
        ViewType View { get; set; }

        Task InitAsync();

        Task<bool> AddIdentityAsync(AddIdentityModel model);

        IReadOnlyCollection<AttachLinkModel> GetAttachLinks(string returnUrl);
    }

    public class IdentitiesViewModel : IIdentitiesViewModel
    {
        private const string ViewKey = "identities_view";
        private const string SortingKey = "identities_sorting";
        private static readonly IdentificationType[] Identifiers =
        {
            IdentificationType.Google,
            IdentificationType.Facebook,
            IdentificationType.Microsoft
        };
        
        private IdentitySorting _sorting = IdentitySorting.None;
        private ViewType _view = ViewType.List;
        
        private List<IdentityModel> _identities = new List<IdentityModel>();

        private readonly IIdentityService _identityService;
        private readonly IToaster _toaster;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _configuration;

        private event Action ViewChanged;
        private event Action SortingChanged;

        public IdentitiesViewModel(IIdentityService identityService, IToaster toaster,
            ILocalStorageService localStorage, IConfiguration configuration)
        {
            _identityService = identityService;
            _toaster = toaster;
            _localStorage = localStorage;
            _configuration = configuration;

            ViewChanged += async () => await localStorage.SetItemAsync(ViewKey, View);
            SortingChanged += async () =>
            {
                SortIdentities();
                await localStorage.SetItemAsync(SortingKey, Sorting);
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

        public async Task InitAsync()
        {
            _view = await _localStorage.GetItemAsync<ViewType>(ViewKey);
            _sorting = await _localStorage.GetItemAsync<IdentitySorting>(SortingKey);

            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var data = await _identityService.GetAsync();

            if (data.IsSuccess)
            {
                _identities = data.Data.ToList();
                SortIdentities();
            }
            else
                _toaster.Error(data.Error?.Detail);
        }

        public async Task<bool> AddIdentityAsync(AddIdentityModel model)
        {
            var result = await _identityService.AddAsync(model);
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
        
        public IReadOnlyCollection<AttachLinkModel> GetAttachLinks(string returnUrl)
        {
            var host = new Uri(_configuration["Urls:IdentitySite"]);

            return Identifiers
                .Select(x => new AttachLinkModel(x, new Uri(host, $"/attach/provider/{x}?returnUrl={returnUrl}")))
                .ToArray();
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

        private void OnSortingChanged() =>
            SortingChanged?.Invoke();
    }
}