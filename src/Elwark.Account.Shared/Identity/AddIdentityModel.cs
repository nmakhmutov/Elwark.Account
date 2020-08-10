using FluentValidation;

namespace Elwark.Account.Shared.Identity
{
    public class AddIdentityModel
    {
        public string? Email { get; set; }
        
        public class Validator : AbstractValidator<AddIdentityModel>
        {
            public Validator()
            {
                CascadeMode = CascadeMode.Stop;
                
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();
            }
        }
    }
}