using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Elwark.Account.Service.Profile.Requests
{
    public sealed record UpdatePasswordRequest
    {
        public string? OldPassword { get; set; }
        
        public string? NewPassword { get; set; }
        
        public string? ConfirmNewPassword { get; set; }
        
        public class Validator : AbstractValidator<UpdatePasswordRequest>
        {
            public Validator(IStringLocalizer<App> l)
            {
                RuleFor(x => x.OldPassword)
                    .NotEmpty()
                    .WithName(l["OldPassword"]);

                RuleFor(x => x.NewPassword)
                    .NotEmpty()
                    .MaximumLength(999)
                    .WithName(l["NewPassword"]);

                RuleFor(x => x.ConfirmNewPassword)
                    .NotEmpty()
                    .Equal(x => x.NewPassword)
                    .WithName(l["ConfirmPassword"]);
            }
        }
    }
}
