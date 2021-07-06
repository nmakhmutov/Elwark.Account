using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Elwark.Account.Infrastructure;
using Elwark.Account.Service.Profile;
using Elwark.Account.States;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using Polly;
using Polly.Extensions.Http;

namespace Elwark.Account
{
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
                .AddSingleton<ProfileStateProvider>()
                .AddSingleton<InfrastructureStateProvider>()
                .AddTransient<AccountLocalization>()
                .AddTransient<AccountAuthorization>();

            builder.Services
                .AddHttpClient<IProfileClient, ProfileClient>(client =>
                    client.BaseAddress = new Uri(builder.Configuration["Urls:Gateway"]!)
                )
                .AddHttpMessageHandler<AccountLocalization>()
                .AddHttpMessageHandler<AccountAuthorization>()
                .AddPolicyHandler(policy);

            builder.Services
                .AddHttpClient<IInfrastructureClient, InfrastructureClient>(client =>
                    client.BaseAddress = new Uri(builder.Configuration["Urls:Gateway"]!)
                )
                .AddHttpMessageHandler<AccountLocalization>()
                .AddHttpMessageHandler<AccountAuthorization>()
                .AddPolicyHandler(policy);

            await builder.Build().RunAsync();
        }
    }
}
