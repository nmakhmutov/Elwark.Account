using Elwark.People.Abstractions;

namespace Elwark.Account.Web.Models
{
    public class ChangeNotificationTypeRequest
    {
        public IdentityId Id { get; set; }
        
        public NotificationType Type { get; set; }
    }
}