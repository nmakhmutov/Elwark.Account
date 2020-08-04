using FluentValidation;

namespace Elwark.Account.Web.Services.PasswordService.Model
{
    public class CreatePasswordModel
    {
        public long? Code { get; set; }

        public string? Password { get; set; }

        public string? Confirmation { get; set; }
        
        public class Validator : AbstractValidator<CreatePasswordModel>
        {
            public Validator()
            {
                RuleFor(x => x.Code)
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