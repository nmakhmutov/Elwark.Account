using System;

namespace Elwark.Account.Service.Profile.Models
{
    public sealed record Profile(
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
        DayOfWeek FirstDayOfWeek,
        Ban? Ban,
        bool IsPasswordAvailable,
        DateTime CreatedAt,
        Connection[] Connections
    );
}
