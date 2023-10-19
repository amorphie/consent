using System.Text.Json.Nodes;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using Refit;

namespace amorphie.consent.Service.Refit;

/// <summary>
/// Payment service to process payment informations
/// </summary>
public interface IPaymentClientService
{
    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Post("/transfers/odeme-emri-rizasi")]
    Task<OdemeEmriRizasiServiceResponseDto> SendOdemeEmriRizasi([Body] OdemeEmriRizaIstegiHHSDto odemeEmriRizaIstegi);
    
    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Post("/transfers/odeme-emri")]
    Task<OdemeEmriHHSDto> SendOdemeEmri([Body]OdemeEmriIstegiHHSDto odemeEmriIstegi);
}