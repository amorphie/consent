using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
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
    public Task<ApiResult> GetAuthorizedAccountConsent(string userTCKN, string yosCode, List<string> permissions);

    /// <summary>
    /// Get User's authorized account consent.
    /// Consent state must be  yetki kullanildi.
    /// Checks yos code, account ref and permissions.
    /// </summary>
    /// <param name="userTCKN">User TCKN number</param>
    /// <param name="yosCode">Yos Code - Bank Code</param>
    /// <param name="permissions">Required Permissions</param>
    /// <param name="accountRef">Account ref</param>
    /// <returns>User's authorized consent</returns>
    public Task<ApiResult> GetAuthorizedAccountConsent(string userTCKN, string yosCode, List<string> permissions,
        string accountRef);

    /// <summary>
    /// Get user consent by checking id, state, yosCode, consentType
    /// Checks consent identity value with given userTCKN.
    /// This metod works for Bireysel consents.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userTCKN"></param>
    /// <param name="consentState"></param>
    /// <param name="consentTypes"></param>
    /// <returns>Consent data</returns>
    public Task<ApiResult> GetConsentReadonly(Guid id, string userTCKN, string consentState,
        List<string> consentTypes);


    /// <summary>
    /// Get active account consents of user - Yetki Bekleniyor, Yetkilendirildi, Yetki Kullanıldı states
    /// Checks identity with consent identity properties
    /// </summary>
    /// <param name="identity">Identity object in account consent</param>
    /// <param name="yosCode">Yos code</param>
    /// <returns>Get active account consents response</returns>
    public Task<ApiResult> GetActiveAccountConsentsOfUser(KimlikDto identity, string yosCode);
}