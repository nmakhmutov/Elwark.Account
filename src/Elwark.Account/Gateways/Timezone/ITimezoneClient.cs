namespace Elwark.Account.Gateways.Timezone;

public interface ITimezoneClient
{
    Task<ApiResponse<Timezone[]>> GetAsync();
}