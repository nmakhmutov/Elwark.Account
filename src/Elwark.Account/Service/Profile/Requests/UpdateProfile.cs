using System;
using Elwark.Account.Service.Profile.Models;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Elwark.Account.Service.Profile.Requests
{
    public sealed record UpdateProfile
    {
        public UpdateProfile(string? firstName, string? lastName, string nickname, string language, Gender gender,
            DateTime? birthday, string? bio, string? countryCode, string? cityName, string timezone)
        {
            FirstName = firstName;
            LastName = lastName;
            Nickname = nickname;
            Language = language;
            Gender = gender;
            Birthday = birthday;
            Bio = bio;
            CountryCode = countryCode;
            CityName = cityName;
            Timezone = timezone;
        }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Nickname { get; set; }

        public string Language { get; set; }

        public Gender Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Bio { get; set; }

        public string? CountryCode { get; set; }

        public string? CityName { get; set; }

        public string Timezone { get; set; }

        public class Validator : AbstractValidator<UpdateProfile>
        {
            public Validator(IStringLocalizer<App> l)
            {
                RuleFor(x => x.Nickname)
                    .MinimumLength(3)
                    .MaximumLength(100)
                    .NotEmpty()
                    .WithName(l["Nickname"]);

                RuleFor(x => x.FirstName)
                    .MaximumLength(100)
                    .WithName(l["FirstName"]);

                RuleFor(x => x.LastName)
                    .MaximumLength(100)
                    .WithName(l["LastName"]);
                
                RuleFor(x => x.Birthday)
                    .NotEmpty()
                    .WithName(l["Birthday"]);

                RuleFor(x => x.Gender)
                    .IsInEnum()
                    .WithName(l["Gender"]);

                RuleFor(x => x.Language)
                    .NotEmpty()
                    .WithName(l["Language"]);

                RuleFor(x => x.CountryCode)
                    .NotEmpty()
                    .WithName(l["Country"]);

                RuleFor(x => x.CityName)
                    .MaximumLength(50)
                    .WithName(l["City"]);

                RuleFor(x => x.Timezone)
                    .NotEmpty()
                    .WithName(l["Timezone"]);

                RuleFor(x => x.Bio)
                    .MaximumLength(260)
                    .WithName(l["Bio"]);
            }
        }
    }
}