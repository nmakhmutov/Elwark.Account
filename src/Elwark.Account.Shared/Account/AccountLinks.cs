using System;
using FluentValidation;

namespace Elwark.Account.Shared.Account
{
    public class AccountLinks
    {
        public string? Github { get; set; }
        
        public string? LinkedIn { get; set; }
        
        public string? Dribbble { get; set; }
        
        public string? Medium { get; set; }
        
        public string? Twitter { get; set; }
        
        public string? Facebook { get; set; }
        
        public string? Website { get; set; }
        
        public class Validator : AbstractValidator<AccountLinks>
        {
            public Validator()
            {
                RuleFor(x => x.Dribbble)
                    .Must(IsUrl).When(x => !string.IsNullOrEmpty(x.Dribbble));
                
                RuleFor(x => x.Facebook)
                    .Must(IsUrl).When(x => !string.IsNullOrEmpty(x.Facebook));
                
                RuleFor(x => x.Github)
                    .Must(IsUrl).When(x => !string.IsNullOrEmpty(x.Github));
                
                RuleFor(x => x.Medium)
                    .Must(IsUrl).When(x => !string.IsNullOrEmpty(x.Medium));
                
                RuleFor(x => x.Twitter)
                    .Must(IsUrl).When(x => !string.IsNullOrEmpty(x.Twitter));
                
                RuleFor(x => x.Website)
                    .Must(IsUrl).When(x => !string.IsNullOrEmpty(x.Website));
                
                RuleFor(x => x.LinkedIn)
                    .Must(IsUrl).When(x => !string.IsNullOrEmpty(x.LinkedIn));
            }
            
            private static bool IsUrl(string? link) =>
                Uri.TryCreate(link, UriKind.Absolute, out var result)
                && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }
    }
}