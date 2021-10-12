using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Elwark.Account.Gateways.Country;
using Elwark.Account.Gateways.Profile;
using Elwark.Account.Gateways.Timezone;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Polly;
using Polly.Extensions.Http;

namespace Elwark.Account;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
            
        builder.Logging.SetMinimumLevel(LogLevel.Critical);
            
        var policy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)));
            
        builder.Services
            .AddMudServices()
            .AddBlazoredLocalStorage()
            .AddLocalization(options => options.ResourcesPath = "Resources");

        builder.Services.AddOidcAuthentication(options =>
            builder.Configuration.Bind("OpenIdConnect", options.ProviderOptions));

        builder.Services
            .AddScoped<ThemeProvider>()
            .AddScoped<LocalizationHandler>()
            .AddScoped<AuthorizationMessageHandler>(provider => 
                new AuthorizationMessageHandler(
                        provider.GetRequiredService<IAccessTokenProvider>(),
                        provider.GetRequiredService<NavigationManager>()
                    )
                    .ConfigureHandler(new[] { builder.Configuration["Urls:Gateway"] })
            );

        builder.Services
            .AddHttpClient<IProfileClient, ProfileClient>(client =>
                client.BaseAddress = new Uri(builder.Configuration["Urls:Gateway"]!)
            )
            .AddHttpMessageHandler<LocalizationHandler>()
            .AddHttpMessageHandler<AuthorizationMessageHandler>()
            .AddPolicyHandler(policy);

        builder.Services
            .AddHttpClient<ICountryClient, CountryClient>(client =>
                client.BaseAddress = new Uri(builder.Configuration["Urls:Gateway"]!)
            )
            .AddHttpMessageHandler<LocalizationHandler>()
            .AddHttpMessageHandler<AuthorizationMessageHandler>()
            .AddPolicyHandler(policy);
            
        builder.Services
            .AddHttpClient<ITimezoneClient, TimezoneClient>(client =>
                client.BaseAddress = new Uri(builder.Configuration["Urls:Gateway"]!)
            )
            .AddHttpMessageHandler<LocalizationHandler>()
            .AddHttpMessageHandler<AuthorizationMessageHandler>()
            .AddPolicyHandler(policy);

        await builder.Build().RunAsync();
    }
}
