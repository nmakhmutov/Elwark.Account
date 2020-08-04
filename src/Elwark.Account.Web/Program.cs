using System;
using System.Threading.Tasks;
using Blazored.Modal;
using Elwark.Account.Web.Pages.Identities.Components.Models;
using Elwark.Account.Web.Pages.Profile.Components;
using Elwark.Account.Web.Services;
using Elwark.Account.Web.Services.AccountService;
using Elwark.Account.Web.Services.AccountService.Model;
using Elwark.Account.Web.Services.IdentityService;
using Elwark.Account.Web.Services.PasswordService;
using Elwark.Account.Web.Services.PasswordService.Model;
using Elwark.Account.Web.State;
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
            
            builder.Services.AddHttpClientServices(builder.Configuration);
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
                .AddBlazoredModal();

            builder.Services
                .AddSingleton<AccountStateProvider>()
                .AddValidators();

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
        public static IServiceCollection AddValidators(this IServiceCollection services) =>
            services.AddTransient<IValidator<UpdatePasswordModel>, UpdatePasswordModel.Validator>()
                .AddTransient<IValidator<CreatePasswordModel>, CreatePasswordModel.Validator>()
                .AddTransient<IValidator<AccountModel>, AccountModel.Validator>()
                .AddTransient<IValidator<EmailIdentityInputModel>, EmailIdentityInputModel.Validator>()
                .AddTransient<IValidator<PictureInputModel>, PictureInputModel.Validator>()
                .AddTransient<IValidator<IdentityConfirmationInputModel>, IdentityConfirmationInputModel.Validator>();

        public static IServiceCollection AddHttpClientServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<AuthorizationDelegatingHandler>();

            services
                .AddHttpClient<IAccountService, AccountService>(client =>
                    client.BaseAddress = new Uri(configuration["Urls:PeopleApi"])
                )
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            services
                .AddHttpClient<IPasswordService, PasswordService>(client =>
                    client.BaseAddress = new Uri(configuration["Urls:PeopleApi"])
                )
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            services
                .AddHttpClient<IIdentityService, IdentityService>(client =>
                    client.BaseAddress = new Uri(configuration["Urls:PeopleApi"])
                )
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            services.AddElwarkStorageClient(new Uri(configuration["Urls:StorageApi"]));

            return services;
        }
    }
}