namespace Elwark.Account.Shared.Password
{
    public abstract class PasswordModel
    {
        public string? Password { get; set; }

        public string? Confirmation { get; set; }
    }
}