namespace Elwark.Account.Gateways.Profile.Requests;

public sealed record UpdateRequest(
    string Nickname,
    string? FirstName, 
    string? LastName,
    bool PreferNickname,
    string Language, 
    string? CountryCode,
    string TimeZone,
    string DateFormat,
    string TimeFormat,
    DayOfWeek StartOfWeek
);
