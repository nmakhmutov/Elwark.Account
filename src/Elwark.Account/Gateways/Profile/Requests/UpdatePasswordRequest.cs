namespace Elwark.Account.Gateways.Profile.Requests;

public sealed record UpdatePasswordRequest(string OldPassword, string NewPassword, string ConfirmNewPassword);
