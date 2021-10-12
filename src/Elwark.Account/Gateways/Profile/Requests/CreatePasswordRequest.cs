namespace Elwark.Account.Gateways.Profile.Requests;

public sealed record CreatePasswordRequest(string Id, uint Code, string Password, string ConfirmPassword);
