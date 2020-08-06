using FluentValidation;

namespace Elwark.Account.Shared.PasswordService.Model
{
    public class UpdatePasswordModel
    {
        public string? Current { get; set; }

        public string? Password { get; set; }

        public string? Confirmation { get; set; }

        public class Validator : AbstractValidator<UpdatePasswordModel>
        {
            public Validator()
            {
                RuleFor(x => x.Current)
                    .NotEmpty();

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