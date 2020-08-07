using Elwark.People.Abstractions;

namespace Elwark.Account.Web.Models
{
    public class ConfirmIdentityRequest
    {
        public IdentityId Id { get; set; }
        
        public long Code { get; set; }
    }
}