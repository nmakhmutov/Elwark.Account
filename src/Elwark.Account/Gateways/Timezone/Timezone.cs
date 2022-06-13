namespace Elwark.Account.Gateways.Timezone;

public record Timezone(string Id, string Name)
{
    public override string ToString() =>
        $"{Id} {Name}";
}
