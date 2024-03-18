using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.MessagingGateway;
using amorphie.consent.core.DTO.OpenBanking;
using Refit;

public interface IDeviceRecordClientService
{
    [Get("/public/CheckDevice/{tckn}")]
    Task<HttpResponseMessage> GetDeviceRecord(string tckn);
}
