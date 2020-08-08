using FluentValidation;

namespace Elwark.Account.Shared.Password
{
    public class CreatePasswordModel : PasswordModel
    {
        public long? Code { get; set; }

        public class Validator : AbstractValidator<CreatePasswordModel>
        {
            public Validator()
            {
                RuleFor(x => x.Code)
                    .NotNull();

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .MinimumLength(6);
                
                RuleFor(x => x.Confirmation)
                    .NotEmpty()
                    .MinimumLength(6)
                    .Equal(x => x.Password).WithMessage("Passwords do not match");
            }
        }
    }
}