using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.Token;
using amorphie.consent.Service.Refit;
using Newtonsoft.Json;

namespace amorphie.consent.Service;
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
            if (!customerDevice.IsSuccessStatusCode)
            {//Error in service
                apiResult.Result = false;
                return apiResult;
            }
            var content = await customerDevice.Content.ReadAsStringAsync();
            var getDeviceResponse = JsonConvert.DeserializeObject<GetDeviceRecordResponseDto>(content);
            apiResult.Data = getDeviceResponse;
        }
        catch (Exception e)
        {
            apiResult.Result = false;
            apiResult.Message = e.Message;
        }
        return apiResult;
    }
}
