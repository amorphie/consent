using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Model;

namespace amorphie.consent.Service.Interface;

public interface IOBAuthorizationService
{

    /// <summary>
    /// Get users yetkikullanildi state of account consents
    /// </summary>
    /// <param name="userTCKN">Consent owner tckn</param>
    /// <param name="context"></param>
    /// <returns>User's account consents</returns>
    public Task<ApiResult> GetAuthUsedAccountConsentsOfUser(string userTCKN);

    /// <summary>
    /// Get User's authorized account consent.
    /// Consent state must be  yetki kullanildi.
    /// Checks yos code and permissions.
    /// </summary>
    /// <param name="userTCKN">User TCKN number</param>
    /// <param name="yosCode">Yos Code - Bank Code</param>
    /// <param name="permissions">Required Permissions</param>
    /// <returns>User's authorized consent</returns>
    public Task<ApiResult> GetAuthorizedAccountConsent(string userTCKN,string yosCode, List<string> permissions);

}