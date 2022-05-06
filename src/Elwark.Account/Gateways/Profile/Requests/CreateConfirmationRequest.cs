using Elwark.Account.Gateways.Profile.Models;

namespace Elwark.Account.Gateways.Profile.Requests;

public sealed record CreateConfirmationRequest(IdentityType Type, string Value);
