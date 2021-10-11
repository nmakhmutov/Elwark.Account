namespace Elwark.Account.Service.Country;

public sealed record Country(string Code, string Name)
{
    public override string ToString() =>
        $"{Name} ({Code})";
}
