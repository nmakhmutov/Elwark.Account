using FluentValidation;

namespace Elwark.Account.Web.Pages.Identities.Components.Models
{
    public class EmailIdentityInputModel
    {
        public string Email { get; set; } = string.Empty;
        
        public class Validator : AbstractValidator<EmailIdentityInputModel>
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