using System;
using FluentValidation;

namespace Elwark.Account.Web.Models
{
    public class PictureInputModel
    {
        public string Picture { get; set; } = string.Empty;
        
        public class Validator : AbstractValidator<PictureInputModel>
        {
            public Validator()
            {
                CascadeMode = CascadeMode.Stop;
                
                RuleFor(x => x.Picture)
                    .NotEmpty()
                    .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _)).WithMessage("Incorrect url");
            }
        }
    }
}