using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Elwark.Account;
using Elwark.Account.Components.Account;
using Elwark.Account.Gateways.Country;
using Elwark.Account.Gateways.Profile;
using Elwark.Account.Gateways.Timezone;
using Elwark.Account.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Polly;
using Polly.Extensions.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Logging.ClearProviders();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddMudServices(configuration =>
    {
        configuration.SnackbarConfiguration.PreventDuplicates = false;
        configuration.SnackbarConfiguration.NewestOnTop = true;
        configuration.SnackbarConfiguration.MaxDisplayedSnackbars = 3;
    })
    .AddBlazoredLocalStorage(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    })
    .AddLocalization(options => options.ResourcesPath = "Resources");

var gatewayUrl = builder.Configuration.GetValue<Uri>("Urls:Gateway")!;
var policy = builder.HostEnvironment.IsDevelopment()
    ? HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryAsync(new[] { TimeSpan.Zero })
    : HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryAsync(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5) });

builder.Services
    .AddOidcAuthentication(options => builder.Configuration.Bind("OpenIdConnect", options.ProviderOptions));

builder.Services
    .AddScoped<ThemeService>()
    .AddScoped<LocalizationHandler>()
    .AddScoped<AccountStateProvider>()
    .AddScoped<AuthorizationMessageHandler>(provider =>
        new AuthorizationMessageHandler(
                provider.GetRequiredService<IAccessTokenProvider>(),
                provider.GetRequiredService<NavigationManager>()
            )
            .ConfigureHandler(new[] { gatewayUrl.ToString() })
    );

builder.Services
    .AddHttpClient<IAccountClient, AccountClient>(client => client.BaseAddress = gatewayUrl)
    .AddHttpMessageHandler<LocalizationHandler>()
    .AddHttpMessageHandler<AuthorizationMessageHandler>()
    .AddPolicyHandler(policy);

var worldUrl = builder.Configuration.GetValue<Uri>("Urls:World.Api")!;
builder.Services
    .AddHttpClient<ICountryClient, CountryClient>(client => client.BaseAddress = worldUrl)
    .AddHttpMessageHandler<LocalizationHandler>()
    .AddPolicyHandler(policy);

builder.Services
    .AddHttpClient<ITimezoneClient, TimezoneClient>(client => client.BaseAddress = worldUrl)
    .AddHttpMessageHandler<LocalizationHandler>()
    .AddPolicyHandler(policy);

var app = builder.Build();

await app.RunAsync();
