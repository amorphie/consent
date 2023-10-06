using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;

namespace amorphie.consent.Service;

public class PaymentService:IPaymentService
{
    private readonly IPaymentClientService _paymentClientService;

    public PaymentService(IPaymentClientService paymentClientService)
    {
        _paymentClientService = paymentClientService;
    } 

    public async Task<OdemeEmriRizasiServiceResponseDto> SendOdemeEmriRizasi(OdemeEmriRizaIstegiHHSDto odemeEmriRizaIstegi)
    {
        OdemeEmriRizasiServiceResponseDto result = new OdemeEmriRizasiServiceResponseDto();
        try
        {
            var serviceResponse = await _paymentClientService.SendOdemeEmriRizasi(odemeEmriRizaIstegi);
            if(serviceResponse["error"] != null)
            {
                result.Error = serviceResponse["error"].ToString();
            }
            else
            {
                result.OdemeEmriRizaIstegi = Newtonsoft.Json.JsonConvert.DeserializeObject<OdemeEmriRizaIstegiHHSDto>(serviceResponse.ToString());
            }
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }
        return result;
    }
}