using System;
using FluentValidation;

namespace Elwark.Account.Web.Pages.Profile.Components
{
    public class PictureInputModel
    {
        public string Picture { get; set; } = string.Empty;
        
        public class Validator : AbstractValidator<PictureInputModel>
        {
            public Validator()
            {
                RuleFor(x => x.Picture)
                    .NotEmpty()
                    .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _)).WithMessage("Incorrect url");
            }
        }
    }
}