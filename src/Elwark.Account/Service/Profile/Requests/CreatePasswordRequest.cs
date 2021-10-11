using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Elwark.Account.Service.Profile.Requests;

public sealed record CreatePasswordRequest
{
    public CreatePasswordRequest(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
        
    public uint? Code { get; set; }
        
    public string? Password { get; set; }
        
    public string? ConfirmPassword { get; set; }

    public class Validator : AbstractValidator<CreatePasswordRequest>
    {
        public Validator(IStringLocalizer<App> l)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithName(l["Code"]);

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