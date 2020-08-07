using Elwark.People.Abstractions;

namespace Elwark.Account.Web.Models
{
    public class AddIdentityRequest
    {
        public AddIdentityRequest(Identification.Email email) =>
            Email = email;

        public Identification.Email Email { get; }
    }
}