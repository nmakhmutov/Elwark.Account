namespace Elwark.Account.Gateways.Profile.Models;

public sealed record Account(
    long Id,
    string Nickname,
    bool PreferNickname,
    string? FirstName,
    string? LastName,
    string FullName,
    string Language,
    string Picture,
    string? CountryCode,
    string TimeZone,
    string DateFormat,
    string TimeFormat,
    DayOfWeek WeekStart,
    DateTime CreatedAt,
    Email[] Emails,
    Connection[] Connections
);

public sealed record Email(string Value, bool IsPrimary, bool IsConfirmed);
    
public sealed record Connection(ExternalService Type, string Identity, string? FirstName, string? LastName);

public enum ExternalService : byte
{
    Unknown = 0,
    Google = 1,
    Microsoft = 2
}
