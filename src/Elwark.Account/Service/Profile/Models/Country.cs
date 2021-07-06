namespace Elwark.Account.Service.Profile.Models
{
    public sealed record Country(string Code, string Name)
    {
        public override string ToString() =>
            $"{Name} ({Code})";
    }
}
