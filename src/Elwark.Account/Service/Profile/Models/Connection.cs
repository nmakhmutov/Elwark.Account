namespace Elwark.Account.Service.Profile.Models
{
    public abstract record Connection(IdentityType IdentityType, string Value, bool IsConfirmed);

    public sealed record EmailConnection(IdentityType IdentityType, string Value, bool IsConfirmed, EmailType EmailType)
        : Connection(IdentityType, Value, IsConfirmed);

    public sealed record SocialConnection(
        IdentityType IdentityType,
        string Value,
        bool IsConfirmed,
        string? FirstName,
        string? LastName
    ) : Connection(IdentityType, Value, IsConfirmed);

    public enum EmailType
    {
        None = 0,
        PrimaryEmail = 1,
        SecondaryEmail = 2
    }
}
