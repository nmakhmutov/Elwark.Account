namespace Elwark.Account.Service.Profile.Models
{
    public abstract record Identity(IdentityType IdentityType, string Value, bool IsConformed);

    public sealed record EmailIdentity(IdentityType IdentityType, string Value, bool IsConformed, EmailType EmailType)
        : Identity(IdentityType, Value, IsConformed);

    public sealed record SocialIdentity(IdentityType IdentityType, string Value, bool IsConformed, string Name)
        : Identity(IdentityType, Value, IsConformed);
    
    public enum EmailType {
        None = 0,
        PrimaryEmail = 1,
        SecondaryEmail = 2
    }
}