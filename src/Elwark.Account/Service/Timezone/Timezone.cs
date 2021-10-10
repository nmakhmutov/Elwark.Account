using System;

namespace Elwark.Account.Service.Timezone
{
    public record Timezone(string Name, TimeSpan Offset)
    {
        public override string ToString() =>
            Name + " " + Offset;
    }
}
