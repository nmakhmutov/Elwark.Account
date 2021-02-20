using System.Collections.Generic;

namespace Elwark.Account
{
    public sealed record InfrastructureState(Dictionary<string, string> Countries, Dictionary<string, string> Timezones);
}