using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Elwark.Account.Infrastructure
{
    public class AccountAuthorization : AuthorizationMessageHandler
    {
        public AccountAuthorization(IAccessTokenProvider provider, NavigationManager navigation, IConfiguration configuration)
            : base(provider, navigation) => ConfigureHandler(new[] {configuration["Urls:Gateway"]});
    }
}
