using System;
using System.Globalization;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Modal;
using Elwark.Account.Shared.Account;
using Elwark.Account.Shared.Identity;
using Elwark.Account.Shared.Password;
using Elwark.Account.Web.Clients;
using Elwark.Account.Web.Handlers;
using Elwark.Account.Web.Models;
using Elwark.Account.Web.State;
using Elwark.Storage.Client;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Sotsera.Blazor.Toaster.Core.Models;

namespace Elwark.Account.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Logging.SetMinimumLevel(builder.HostEnvironment.IsDevelopment()
                ? LogLevel.Trace
                : LogLevel.Critical);

            builder.Services
                .AddValidators()
                .AddViewModels()
                .AddHttpClientServices(builder.Configuration);

            builder.Services.AddToaster(configuration =>
                {
                    configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    configuration.PreventDuplicates = true;
                    configuration.NewestOnTop = false;
                    configuration.ShowTransitionDuration = 600;
                    configuration.VisibleStateDuration = 5000;
                    configuration.HideTransitionDuration = 600;
                    configuration.ShowProgressBar = false;
                })
                .AddBlazoredModal()
                .AddBlazoredLocalStorage()
                .AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services
                .AddOidcAuthentication(options =>
                {
                    options.ProviderOptions.DefaultScopes.Clear();
                    builder.Configuration.Bind("OpenIdConnect", options.ProviderOptions);
                });

            var host = builder.Build();
            
            await host.Services.GetRequiredService<LocalizationState>().Init();

            await host.RunAsync();
        }
    }

    public static class ProgramExtensions
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) =>
            services
                .AddSingleton<AccountStore>()
                .AddScoped<LocalizationState>()
                .AddScoped<AccountState>()
                .AddScoped<PasswordState>()
                .AddScoped<IdentitiesState>();

        public static IServiceCollection AddValidators(this IServiceCollection services) =>
            services.AddTransient<IValidator<UpdatePasswordModel>, UpdatePasswordModel.Validator>()
                .AddTransient<IValidator<CreatePasswordModel>, CreatePasswordModel.Validator>()
                .AddTransient<IValidator<AccountModel>, AccountModel.Validator>()
                .AddTransient<IValidator<AddIdentityModel>, AddIdentityModel.Validator>()
                .AddTransient<IValidator<PictureInputModel>, PictureInputModel.Validator>()
                .AddTransient<IValidator<ConfirmIdentityModel>, ConfirmIdentityModel.Validator>();

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<AuthorizationDelegatingHandler>();

            services.AddHttpClient<IAccountClient, AccountClient>(client =>
                    client.BaseAddress = new Uri(configuration["Urls:PeopleApi"])
                )
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            services.AddHttpClient<IPasswordClient, PasswordClient>(client =>
                    client.BaseAddress = new Uri(configuration["Urls:PeopleApi"])
                )
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            services.AddHttpClient<IIdentityClient, IdentityClient>(client =>
                    client.BaseAddress = new Uri(configuration["Urls:PeopleApi"])
                )
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            services.AddElwarkStorageClient(new Uri(configuration["Urls:StorageApi"]));

            return services;
        }
    }
}