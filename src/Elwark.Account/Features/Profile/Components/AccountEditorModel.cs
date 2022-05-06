using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Elwark.Account.Features.Profile.Components;

public sealed record AccountEditorModel(string? FirstName, string? LastName, string Nickname, bool PreferNickname,
    string Language, string? CountryCode, string Timezone, string DateFormat, string TimeFormat,
    DayOfWeek WeekStart, string Picture, string FullName, DateTime CreatedAt)
{
    public string? FirstName { get; set; } = FirstName;

    public string? LastName { get; set; } = LastName;

    public string Nickname { get; set; } = Nickname;

    public bool PreferNickname { get; set; } = PreferNickname;

    public string Language { get; set; } = Language;

    public string? CountryCode { get; set; } = CountryCode;

    public string Timezone { get; set; } = Timezone;

    public DayOfWeek WeekStart { get; set; } = WeekStart;
    
    public string DateFormat { get; set; } = DateFormat;
    
    public string TimeFormat { get; set; } = TimeFormat;

    public class Validator : AbstractValidator<AccountEditorModel>
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
            
            RuleFor(x => x.DateFormat)
                .NotEmpty()
                .WithName(l["DateFormat"]);
            
            RuleFor(x => x.TimeFormat)
                .NotEmpty()
                .WithName(l["DateFormat"]);
            
        }
    }
}
