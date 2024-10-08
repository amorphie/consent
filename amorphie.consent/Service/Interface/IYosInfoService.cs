using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Model;

namespace amorphie.consent.Service.Interface;

public interface IYosInfoService
{

    /// <summary>
    /// Checks if any yos in the system with identified yosCode
    /// </summary>
    /// <param name="yosCode">To be checked yosCode</param>
    /// <returns>Any yos in the system with given yosCode</returns>
    public Task<ApiResult> IsYosInApplication(string yosCode);

    /// <summary>
    /// Gets Yos entity by yosCode
    /// </summary>
    /// <param name="yosCode">Yos code to get info</param>
    /// <returns>Get Yos entity from db response</returns>
    public Task<ApiResult> GetYosByCode(string yosCode);

    /// <summary>
    /// Get yos's public key.
    /// </summary>
    /// <param name="yosCode">Yos Code</param>
    /// <returns>Yos public key</returns>
    public Task<ApiResult> GetYosPublicKey(string yosCode);


    /// <summary>
    /// Check yos by code, if has desired role
    /// </summary>
    /// <param name="yosCode">Yos Code</param>
    /// <param name="eventTypeSourceTypeRelationYosRole">Required yos role</param>
    /// <returns>If yos has desired role</returns>
    public Task<ApiResult> CheckIfYosHasDesiredRole(string yosCode,
        string eventTypeSourceTypeRelationYosRole);


    /// <summary>
    /// Checks If any yos providing given api by yoscode
    /// </summary>
    /// <param name="yosCode">YosCode</param>
    /// <param name="apiName">To be checked api name</param>
    /// <returns>Any yos with given yoscode in system provides desired api</returns>
    public Task<ApiResult> CheckIfYosProvidesDesiredApi(string yosCode,
        string apiName);

    /// <summary>
    /// Check if yos has subscription
    /// </summary>
    /// <param name="yosKod">Yos to be checked</param>
    /// <param name="eventType">Event Type</param>
    /// <param name="sourceType">Source Type</param>
    /// <returns>If YOS has subscription for given event type and source type</returns>
    public Task<ApiResult> IsYosSubscsribed(string yosKod, string eventType, string sourceType);

    /// <summary>
    /// Check if yos has desired address by given authorization type
    /// </summary>
    /// <param name="yosKod">Yos to be checked</param>
    /// <param name="authType">Authorization type (Yonlendirmeli or Ayrik)</param>
    /// <param name="address">Adddress</param>
    /// <returns>if yos has desired address by given authorization type</returns>
    public Task<ApiResult> IsYosAddressCorrect(string yosCode, string authType, string address);

    /// <summary>
    /// Get yos from bkm service by yoscode
    /// Save result to system
    /// </summary>
    /// <param name="yosCode">To be processed yos code</param>
    /// <returns>Save yos code result</returns>
    public Task<ApiResult> SaveYos(string yosCode);

}