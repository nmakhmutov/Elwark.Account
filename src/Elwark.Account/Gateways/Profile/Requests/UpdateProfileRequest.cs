namespace Elwark.Account.Gateways.Profile.Requests;

public sealed record UpdateProfileRequest(
    string? FirstName, 
    string? LastName,
    string Nickname,
    bool PreferNickname,
    string Language, 
    string? CountryCode,
    string TimeZone,
    string DateFormat,
    string TimeFormat,
    DayOfWeek WeekStart
);
