using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;

namespace amorphie.consent.Service.Interface;

public interface IPaymentService
{
    /// <summary>
    /// Send OdemeEmriRizasi object to service to check odeme-emri-rizasi post process
    /// </summary>
    /// <param name="odemeEmriRizaIstegi">Object to be checked</param>
    /// <returns>odeme-emri-rizasi post process result</returns>
    Task<ApiResult> SendOdemeEmriRizasi(OdemeEmriRizaIstegiHHSDto odemeEmriRizaIstegi);

    /// <summary>
    /// Send OdemeEmri object to service to process odeme-emri post 
    /// </summary>
    /// <param name="odemeEmriIstegi">Object to be send</param>
    /// <returns>odeme-emri post process result</returns>
    Task<ApiResult> SendOdemeEmri(OdemeEmriIstegiHHSDto odemeEmriIstegi);

}