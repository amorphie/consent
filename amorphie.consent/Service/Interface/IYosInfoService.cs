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
    /// Check yos by code, if has desired role
    /// </summary>
    /// <param name="yosCode">Yos Code</param>
    /// <param name="abonelikTipleri">Wanted subscription types</param>
    /// <param name="eventTypeSourceTypeRelations">Information Object</param>
    /// <returns>Any yos having desired role by yoscode</returns>
    public Task<ApiResult> CheckIfYosHasDesiredRole(string yosCode,
        List<AbonelikTipleriDto> abonelikTipleri,
        List<OBEventTypeSourceTypeRelation> eventTypeSourceTypeRelations);


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

}