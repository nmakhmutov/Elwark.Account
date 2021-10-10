using Elwark.Account.Service.Country;
using Elwark.Account.Service.Timezone;

namespace Elwark.Account.States
{
    public sealed record InfrastructureState(Country[] Countries, Timezone[] Timezones);
}
