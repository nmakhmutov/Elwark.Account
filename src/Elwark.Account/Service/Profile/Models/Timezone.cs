using System;

namespace Elwark.Account.Service.Profile.Models
{
    public record Timezone(string Name, TimeSpan Offset)
    {
        public override string ToString() =>
            Name + " " + Offset;
    }
    
    public sealed record TimeInfo(Timezone Timezone, DayOfWeek FirstDayOfWeek);
}
