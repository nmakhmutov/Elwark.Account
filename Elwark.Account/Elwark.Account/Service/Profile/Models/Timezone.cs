using System;

namespace Elwark.Account.Service.Profile.Models
{
    public record Timezone(string Name, TimeSpan Offset);
}