using System.Collections.Generic;

namespace Elwark.Account.States
{
    public sealed record InfrastructureState(Dictionary<string, string> Countries, Dictionary<string, string> Timezones);
}