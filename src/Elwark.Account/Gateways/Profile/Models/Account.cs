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
