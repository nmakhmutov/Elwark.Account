using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Elwark.Account.Features.Profile.Components;

public sealed record PasswordCreateEditorModel(string Id)
{
    public uint? Code { get; set; }
        
    public string? Password { get; set; }
        
    public string? ConfirmPassword { get; set; }

    public class Validator : AbstractValidator<PasswordCreateEditorModel>
    {
        public Validator(IStringLocalizer<App> l)
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithName(l["ConfirmationCode"]);

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(999)
                .WithName(l["Password"]);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password)
                .WithName(l["ConfirmPassword"]);
        }
    }
}
