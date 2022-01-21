namespace Elwark.Account.Gateways.Profile.Models;

public sealed record Ban(string Reason, DateTime? ExpiredAt);