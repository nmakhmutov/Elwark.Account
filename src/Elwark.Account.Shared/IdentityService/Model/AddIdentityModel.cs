using Elwark.People.Abstractions;
using FluentValidation;

namespace Elwark.Account.Shared.IdentityService.Model
{
    public class AddIdentityModel
    {
        public Identification.Email? EmailIdentification => string.IsNullOrEmpty(Email)
            ? null
            : new Identification.Email(Email);

        public string? Email { get; set; }
        
        public class Validator : AbstractValidator<AddIdentityModel>
        {
            public Validator()
            {
                CascadeMode = CascadeMode.StopOnFirstFailure;
                
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();
            }
        }
    }
}