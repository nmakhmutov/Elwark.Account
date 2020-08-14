using System.Globalization;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Elwark.Account.Web.State
{
    public class LocalizationState
    {
        private const string Key = "app.language";

        public static readonly CultureInfo[] SupportedCultures =
        {
            new CultureInfo("en"),
            new CultureInfo("ru")
        };

        private readonly NavigationManager _navigation;
        private readonly ILocalStorageService _storage;

        public LocalizationState(ILocalStorageService storage, NavigationManager navigation)
        {
            _storage = storage;
            _navigation = navigation;
        }

        public async Task Init()
        {
            var language = await _storage.GetItemAsStringAsync(Key);
            if (!string.IsNullOrEmpty(language))
            {
                var culture = new CultureInfo(language);
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
        }

        public async Task Set(CultureInfo culture)
        {
            await _storage.SetItemAsync(Key, culture.TwoLetterISOLanguageName);
            _navigation.NavigateTo(_navigation.Uri, true);
        }
    }
}