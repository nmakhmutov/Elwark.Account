using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Modal;
using Elwark.Account.Shared.AccountService;
using Elwark.Account.Shared.AccountService.Model;
using Elwark.Account.Shared.IdentityService;
using Elwark.Account.Shared.IdentityService.Model;
using Elwark.Account.Shared.Password;
using Elwark.Account.Web.Clients;
using Elwark.Account.Web.Handlers;
using Elwark.Account.Web.Pages.Profile.Components;
using Elwark.Account.Web.State;
using Elwark.Account.Web.ViewModels;
using Elwark.Storage.Client;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Toaster.Core.Models;

namespace Elwark.Account.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            
            builder.Services
                .AddSingleton<AccountStateProvider>()
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
                .AddBlazoredLocalStorage();

            builder.Services
                .AddOidcAuthentication(options =>
                {
                    options.ProviderOptions.DefaultScopes.Clear();
                    builder.Configuration.Bind("OpenIdConnect", options.ProviderOptions);
                });

            await builder.Build()
                .RunAsync();
        }
    }

    public static class ProgramExtensions
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) =>
            services
                .AddScoped<PasswordState>()
                .AddTransient<IIdentityViewModel, IdentityViewModel>()
                .AddScoped<IIdentitiesViewModel, IdentitiesViewModel>();
        
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

            services.AddHttpClient<IAccountService, AccountService>(client =>
                    client.BaseAddress = new Uri(configuration["Urls:PeopleApi"])
                )
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            services.AddHttpClient<IPasswordClient, PasswordClient>(client =>
                    client.BaseAddress = new Uri(configuration["Urls:PeopleApi"])
                )
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            services.AddHttpClient<IIdentityService, IdentityService>(client =>
                    client.BaseAddress = new Uri(configuration["Urls:PeopleApi"])
                )
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            services.AddElwarkStorageClient(new Uri(configuration["Urls:StorageApi"]));

            return services;
        }
    }
}