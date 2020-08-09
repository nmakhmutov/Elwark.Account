using FluentValidation;

namespace Elwark.Account.Shared.Identity
{
    public class ConfirmIdentityModel
    {
        public long? Code { get; set; }
        
        public class Validator : AbstractValidator<ConfirmIdentityModel>
        {
            public Validator()
            {
                RuleFor(x => x.Code)
                    .NotNull()
                    .GreaterThan(0);
            }
        }
    }
}