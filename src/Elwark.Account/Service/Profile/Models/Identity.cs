namespace Elwark.Account.Service.Profile.Models
{
    public abstract record Identity(IdentityType IdentityType, string Value, bool IsConfirmed);

    public sealed record EmailIdentity(IdentityType IdentityType, string Value, bool IsConfirmed, EmailType EmailType)
        : Identity(IdentityType, Value, IsConfirmed);

    public sealed record SocialIdentity(IdentityType IdentityType, string Value, bool IsConfirmed, string Name)
        : Identity(IdentityType, Value, IsConfirmed);
    
    public enum EmailType {
        None = 0,
        PrimaryEmail = 1,
        SecondaryEmail = 2
    }
}