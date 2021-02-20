using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Elwark.Account.Infrastructure;
using Elwark.Account.Service.Profile;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace Elwark.Account
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            
            builder.Logging
                .SetMinimumLevel(LogLevel.Critical);
            
            builder.Services
                .AddMudServices()
                .AddBlazoredLocalStorage();

            builder.Services.AddOidcAuthentication(options =>
                builder.Configuration.Bind("OpenIdConnect", options.ProviderOptions));

            builder.Services
                .AddSingleton<ProfileStateProvider>()
                .AddTransient<AccountLocalization>()
                .AddTransient<AccountAuthorization>();

            builder.Services
                .AddHttpClient<IProfileClient, ProfileClient>(client =>
                    client.BaseAddress = new Uri(builder.Configuration["Urls:Gateway"])
                )
                .AddHttpMessageHandler<AccountLocalization>()
                .AddHttpMessageHandler<AccountAuthorization>();

            builder.Services
                .AddHttpClient<IInfrastructureClient, InfrastructureClient>(client =>
                    client.BaseAddress = new Uri(builder.Configuration["Urls:Gateway"])
                )
                .AddHttpMessageHandler<AccountLocalization>()
                .AddHttpMessageHandler<AccountAuthorization>();

            await builder.Build().RunAsync();
        }
    }
}