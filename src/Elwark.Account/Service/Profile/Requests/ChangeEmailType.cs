using Elwark.Account.Service.Profile.Models;

namespace Elwark.Account.Service.Profile.Requests
{
    public sealed record ChangeEmailType(string Email, EmailType Type);
}