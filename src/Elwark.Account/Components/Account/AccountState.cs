using Elwark.Account.Gateways.Profile.Models;

namespace Elwark.Account.Components.Account;

internal sealed record AccountState
{
    public bool IsInitialized { get; init; }

    public string Nickname { get; init; } = string.Empty;

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string FullName { get; init; } = string.Empty;

    public bool PreferNickname { get; init; }

    public string Picture { get; init; } = string.Empty;

    public string Language { get; init; } = string.Empty;

    public string? CountryCode { get; init; }

    public string TimeZone { get; init; } = string.Empty;

    public DayOfWeek WeekStart { get; init; }

    public string DateFormat { get; init; } = string.Empty;

    public string TimeFormat { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public Email[] Emails { get; init; } = Array.Empty<Email>();

    public Connection[] Connections { get; init; } = Array.Empty<Connection>();
}
