namespace Elwark.Account.Gateways.Profile.Models;

public enum ExternalService : byte
{
    Unknown = 0,
    Google = 1,
    Microsoft = 2
}

internal static class ExternalServiceExtensions
{
    internal static string ToFastString(this ExternalService service) =>
        service switch
        {
            ExternalService.Unknown => nameof(ExternalService.Unknown),
            ExternalService.Google => nameof(ExternalService.Google),
            ExternalService.Microsoft => nameof(ExternalService.Microsoft),
            _ => throw new ArgumentOutOfRangeException(nameof(service), service, null)
        };
}
