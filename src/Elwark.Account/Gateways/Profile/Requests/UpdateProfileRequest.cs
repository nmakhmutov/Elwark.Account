using System;

namespace Elwark.Account.Gateways.Profile.Requests;

public sealed record UpdateProfileRequest(
    string? FirstName,
    string? LastName,
    string Nickname,
    bool PreferNickname,
    string Language,
    string? CountryCode,
    string Timezone,
    DayOfWeek FirstDayOfWeek
);
