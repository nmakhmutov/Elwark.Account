namespace Elwark.Account.Gateways.Timezone;

public record Timezone(string Name, TimeSpan Offset)
{
    public override string ToString() =>
        $"{Name} {Offset}";
}
