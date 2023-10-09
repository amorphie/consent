using amorphie.consent.core.DTO;
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

    public async Task<ApiResult> SendOdemeEmriRizasi(OdemeEmriRizaIstegiHHSDto odemeEmriRizaIstegi)
    {
        ApiResult result = new ApiResult();
        try
        {
            //Send odemeemririzasi to servie
            OdemeEmriRizasiServiceResponseDto serviceResponse = await _paymentClientService.SendOdemeEmriRizasi(odemeEmriRizaIstegi);
            if (string.IsNullOrEmpty(serviceResponse.Error))//Success
            {
                result.Data = serviceResponse.OdemeEmriRizaIstegi;
            }
            else
            {//Error in service
                result.Result = false;
                result.Message = serviceResponse.Error;
            }
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }
}