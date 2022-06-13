namespace Elwark.Account.Gateways.Country;

public sealed record Country(string Code, string Name)
{
    public string Flag =>
        $"https://flagcdn.com/{Code.ToLowerInvariant()}.svg";
    
    public override string ToString() =>
        $"{Name} ({Code})";
}
