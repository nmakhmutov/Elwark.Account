namespace Elwark.Account.Gateways.Profile.Models;

public sealed record Connection(ExternalService Type, string Identity, string? FirstName, string? LastName);
