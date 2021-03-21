using FluentValidation;

namespace Elwark.Account.Service.Profile.Requests
{
    public sealed record Confirm
    {
        public sealed class Validator : AbstractValidator<Confirm>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();

                RuleFor(x => x.Code)
                    .NotEmpty();
            }
        }

        public string Id { get; set; } = string.Empty;

        public uint? Code { get; set; }
    };
}