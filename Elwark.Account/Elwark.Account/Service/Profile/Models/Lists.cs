namespace Elwark.Account.Service.Profile.Models
{
    public sealed record Lists(Country[] Countries, Timezone[] Timezones);
    
    public sealed record Country(string Code, string Name);
}