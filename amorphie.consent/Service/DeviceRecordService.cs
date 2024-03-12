using amorphie.consent.core.DTO;
using amorphie.consent.Service.Refit;

public class DeviceRecordService : IDeviceRecord
{
    private readonly IDeviceRecordClientService _deviceRecordClientService;
    public DeviceRecordService(IDeviceRecordClientService deviceRecordClientService)
    {
        _deviceRecordClientService = deviceRecordClientService;
    }
    public async Task<ApiResult> GetDeviceRecord(string tckn)
    {
        ApiResult apiResult = new();
        try
        {
            var customerDevice = await _deviceRecordClientService.GetDeviceRecord(tckn);
            apiResult.Result = customerDevice.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            apiResult.Result = false;
            apiResult.Message = e.Message;
        }
        return apiResult;
    }
}
