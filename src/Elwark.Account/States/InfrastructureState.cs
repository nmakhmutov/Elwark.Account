using Elwark.Account.Service.Profile.Models;

namespace Elwark.Account.States
{
    public sealed record InfrastructureState(Country[] Countries, Timezone[] Timezones);
}
