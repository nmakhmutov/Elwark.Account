using Elwark.People.Abstractions;

namespace Elwark.Account.Web.Models
{
    public class SendConfirmationCodeRequest
    {
        public IdentityId Id { get; set; }
    }
}