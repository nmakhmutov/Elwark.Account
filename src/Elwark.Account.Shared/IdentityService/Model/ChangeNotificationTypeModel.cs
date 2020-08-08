using Elwark.People.Abstractions;

namespace Elwark.Account.Shared.IdentityService.Model
{
    public class ChangeNotificationTypeModel
    {
        public IdentityId Id { get; set; }
        
        public NotificationType Type { get; set; }
    }
}