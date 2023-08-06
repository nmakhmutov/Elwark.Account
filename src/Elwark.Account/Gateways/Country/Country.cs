namespace Elwark.Account.Gateways.Country;

public sealed record Country(string Alpha2, string Alpha3, string Region, string Name)
{
    public string Flag =>
        $"https://flagcdn.com/{Alpha2.ToLowerInvariant()}.svg";
    
    public override string ToString() =>
        $"{Name} ({Alpha2})";
}
