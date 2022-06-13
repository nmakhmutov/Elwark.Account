using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Elwark.Account.Pages.Profile.Components;

public sealed record AccountEditorModel
{
    public string Nickname { get; set; } = string.Empty;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool PreferNickname { get; set; }

    public string Language { get; set; } = string.Empty;

    public string? CountryCode { get; set; }

    public string TimeZone { get; set; } = string.Empty;

    public DayOfWeek WeekStart { get; set; }

    public string DateFormat { get; set; } = string.Empty;

    public string TimeFormat { get; set; } = string.Empty;

    public class Validator : AbstractValidator<AccountEditorModel>
    {
        public Validator(IStringLocalizer<App> l)
        {
            RuleFor(x => x.Nickname)
                .MinimumLength(3)
                .MaximumLength(64)
                .NotEmpty()
                .WithName(l["Nickname"]);

            RuleFor(x => x.FirstName)
                .MaximumLength(128)
                .WithName(l["FirstName"]);

            RuleFor(x => x.LastName)
                .MaximumLength(128)
                .WithName(l["LastName"]);

            RuleFor(x => x.Language)
                .NotEmpty()
                .Length(2)
                .WithName(l["Language"])
                .Must(x => StaticData.Languages.ContainsKey(x));

            RuleFor(x => x.CountryCode)
                .NotEmpty()
                .Length(2)
                .WithName(l["Country"]);

            RuleFor(x => x.TimeZone)
                .NotEmpty()
                .MaximumLength(50)
                .WithName(l["Timezone"]);

            RuleFor(x => x.DateFormat)
                .NotEmpty()
                .WithName(l["DateFormat"])
                .Must(x => StaticData.DateFormats.Contains(x));

            RuleFor(x => x.TimeFormat)
                .NotEmpty()
                .WithName(l["DateFormat"])
                .Must(x => StaticData.TimeFormats.Contains(x));
        }
    }
}
