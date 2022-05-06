using Elwark.Account.Gateways.Profile.Models;

namespace Elwark.Account.Gateways.Profile.Requests;

public sealed record ConfirmConnectionRequest(
    IdentityType Type,
    string Value,
    string ConfirmationToken,
    uint ConfirmationCode
);
