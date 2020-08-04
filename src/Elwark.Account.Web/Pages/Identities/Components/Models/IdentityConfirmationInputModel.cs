using FluentValidation;

namespace Elwark.Account.Web.Pages.Identities.Components.Models
{
    public class IdentityConfirmationInputModel
    {
        public long? Code { get; set; }
        
        public class Validator : AbstractValidator<IdentityConfirmationInputModel>
        {
            public Validator()
            {
                RuleFor(x => x.Code)
                    .NotEmpty()
                    .GreaterThan(0);
            }
        }
    }
}