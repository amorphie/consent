using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;

namespace amorphie.consent.Service.Interface;

public interface IBKMService
{
   
    /// <summary>
    /// Get token from bkm service by given service scope
    /// </summary>
    /// <param name="bkmServiceScope">Service scope</param>
    /// <returns>Get Token service response message</returns>
    Task<ApiResult> GetToken(string bkmServiceScope);
    
    /// <summary>
    /// Send event to YOS
    /// post olay-dinleme endpoint process
    /// </summary>
    /// <param name="olayIstegi">To be post data</param>
    /// <returns>Post olay-dinleme service response</returns>
    Task<ApiResult> SendEventToYos(OlayIstegiDto olayIstegi);

    Task<ApiResult> GetAllHhs();

    Task<ApiResult> GetAllYos();

    Task<ApiResult> GetHhs(string yosKod);
    
}