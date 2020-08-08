using Elwark.People.Abstractions;
using FluentValidation;

namespace Elwark.Account.Shared.IdentityService.Model
{
    public class ConfirmIdentityModel
    {
        public IdentityId Id { get; set; }
        
        public long? Code { get; set; }
        
        public class Validator : AbstractValidator<ConfirmIdentityModel>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotNull();
                
                RuleFor(x => x.Code)
                    .NotNull()
                    .GreaterThan(0);
            }
        }
    }
}