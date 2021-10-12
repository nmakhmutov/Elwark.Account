namespace Elwark.Account.Gateways.Profile.Requests;

public sealed record ConfirmRequest(string Id, uint? Code);
