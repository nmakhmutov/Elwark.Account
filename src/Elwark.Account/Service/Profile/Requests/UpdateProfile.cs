using System;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Elwark.Account.Service.Profile.Requests
{
    public sealed record UpdateProfile
    {
        public UpdateProfile(string? firstName, string? lastName, string nickname, bool preferNickname,
            string language, string? countryCode, string timeZone, DayOfWeek firstDayOfWeek)
        {
            FirstName = firstName;
            LastName = lastName;
            Nickname = nickname;
            PreferNickname = preferNickname;
            Language = language;
            CountryCode = countryCode;
            Timezone = timeZone;
            FirstDayOfWeek = firstDayOfWeek;
        }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Nickname { get; set; }
        
        public bool PreferNickname { get; set; }

        public string Language { get; set; }

        public string? CountryCode { get; set; }

        public string Timezone { get; set; }
        
        public DayOfWeek FirstDayOfWeek { get; set; }

        public class Validator : AbstractValidator<UpdateProfile>
        {
            public Validator(IStringLocalizer<App> l)
            {
                RuleFor(x => x.Nickname)
                    .MinimumLength(3)
                    .MaximumLength(99)
                    .NotEmpty()
                    .WithName(l["Nickname"]);

                RuleFor(x => x.FirstName)
                    .MaximumLength(99)
                    .WithName(l["FirstName"]);

                RuleFor(x => x.LastName)
                    .MaximumLength(99)
                    .WithName(l["LastName"]);

                RuleFor(x => x.Language)
                    .NotEmpty()
                    .WithName(l["Language"]);

                RuleFor(x => x.CountryCode)
                    .NotEmpty()
                    .WithName(l["Country"]);

                RuleFor(x => x.Timezone)
                    .NotEmpty()
                    .MaximumLength(50)
                    .WithName(l["Timezone"]);
            }
        }
    }
}
