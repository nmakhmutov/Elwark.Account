using System;
using FluentValidation;

namespace Elwark.Account.Service.Profile.Models
{
    public class UpdateProfile
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
            public Validator()
            {
                RuleFor(x => x.Nickname)
                    .MinimumLength(3)
                    .NotEmpty();

                RuleFor(x => x.Birthday)
                    .NotEmpty();

                RuleFor(x => x.Gender)
                    .IsInEnum();

                RuleFor(x => x.Language)
                    .NotEmpty();

                RuleFor(x => x.CountryCode)
                    .NotEmpty();

                RuleFor(x => x.Timezone)
                    .NotEmpty();

                RuleFor(x => x.Bio)
                    .MaximumLength(260);
            }
        }
    }
}