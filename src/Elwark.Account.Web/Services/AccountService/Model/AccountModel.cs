using System;
using Elwark.People.Abstractions;
using FluentValidation;

namespace Elwark.Account.Web.Services.AccountService.Model
{
    public class AccountModel
    {
        public Gender Gender { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? FullName { get; set; }

        public string Nickname { get; set; } = string.Empty;

        public DateTime? Birthdate { get; set; }

        public string Picture { get; set; } = string.Empty;

        public string Timezone { get; set; } = string.Empty;

        public string? CountryCode { get; set; }

        public string Language { get; set; } = string.Empty;

        public string? City { get; set; }

        public string? Bio { get; set; }
        
        public AccountLinks Links { get; set; } = new AccountLinks();

        public DateTimeOffset CreatedAt { get; set; }

        public class Validator : AbstractValidator<AccountModel>
        {
            public Validator()
            {
                RuleFor(x => x.Gender)
                    .NotNull()
                    .IsInEnum();

                RuleFor(x => x.Nickname)
                    .NotEmpty();

                RuleFor(x => x.Timezone)
                    .NotEmpty();

                RuleFor(x => x.Picture)
                    .NotEmpty()
                    .Must(IsUrl);

                RuleFor(x => x.CountryCode)
                    .NotEmpty()
                    .Length(2);

                RuleFor(x => x.Language)
                    .NotEmpty()
                    .Length(2);

                RuleFor(x => x.Links)
                    .SetValidator(new AccountLinks.Validator());
            }

            private static bool IsUrl(string link) =>
                Uri.TryCreate(link, UriKind.Absolute, out var result)
                && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }
    }
}