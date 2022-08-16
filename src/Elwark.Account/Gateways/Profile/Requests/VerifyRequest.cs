namespace Elwark.Account.Gateways.Profile.Requests;

public sealed record VerifyRequest(string Token, string Code);
