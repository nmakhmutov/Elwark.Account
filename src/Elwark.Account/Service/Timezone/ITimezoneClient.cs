using System.Threading.Tasks;

namespace Elwark.Account.Service.Timezone
{
    public interface ITimezoneClient
    {
        Task<ApiResponse<Timezone[]>> GetAsync();
    }
}
