namespace Elwark.Account.Gateways.Profile.Models;

public sealed record Email(string Value, bool IsPrimary, bool IsConfirmed);
