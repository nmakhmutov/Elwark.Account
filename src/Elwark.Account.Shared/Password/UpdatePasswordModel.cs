using FluentValidation;

namespace Elwark.Account.Shared.Password
{
    public class UpdatePasswordModel : PasswordModel
    {
        public string? Current { get; set; }

        public class Validator : AbstractValidator<UpdatePasswordModel>
        {
            public Validator()
            {
                RuleFor(x => x.Current)
                    .NotEmpty();

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .NotEqual(x => x.Password).WithMessage("Current and new passwords cannot be equals")
                    .MinimumLength(6);
                
                RuleFor(x => x.Confirmation)
                    .NotEmpty()
                    .MinimumLength(6)
                    .Equal(x => x.Password).WithMessage("Passwords do not match");
            }
        }
    }
}