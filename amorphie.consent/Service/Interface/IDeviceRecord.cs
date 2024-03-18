using amorphie.consent.core.DTO;
using Refit;

namespace amorphie.consent.Service.Refit;

public interface IDeviceRecord
{
    Task<ApiResult> GetDeviceRecord(string tckn);
}
