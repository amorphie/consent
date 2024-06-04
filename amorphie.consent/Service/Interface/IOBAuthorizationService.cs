using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;

namespace amorphie.consent.Service.Interface;

public interface IOBAuthorizationService
{

    /// <summary>
    /// Get users yetkikullanildi state of account consents
    /// Also Checks consent validity date
    /// </summary>
    /// <param name="userTckn">Consent owner tckn</param>
    /// <returns>User's account consents</returns>
    public Task<ApiResult> GetAuthUsedAccountConsentsOfUser(string userTckn);

    /// <summary>
    /// Get users yetkikullanildi state of account consents
    /// Also Checks consent validity date
    /// </summary>
    /// <param name="customerNumber">Consent customer number</param>
    /// <param name="institutionCustomerNumber">Consent institution customer number</param>
    /// <returns>User's account consents</returns>
    public Task<ApiResult> GetAuthUsedAccountConsentsOfInstitutionUser(string customerNumber, string institutionCustomerNumber);

    /// <summary>
    /// Get yetki kullanıldı state of account consent by given id and permission.
    /// Also Checks consent validity date
    /// </summary>
    /// <param name="consentId">Consent Id</param>
    /// <param name="accountRef">To be checked account ref</param>
    /// <param name="permissions">Desired permissions</param>
    /// <returns>Account consent</returns>
    public Task<ApiResult> GetAuthUsedAccountConsent(string consentId, string accountRef, List<string> permissions);

    /// <summary>
    /// Get User's account consent by consent id, tckn, yoscode, permissions.
    /// </summary>
    /// <param name="consentId">Consent id</param>
    /// <param name="userTckn">User TCKN number</param>
    /// <param name="yosCode">Yos Code - Bank Code</param>
    /// <param name="permissions">Required Permissions</param>
    /// <returns>User's account consent</returns>
    public Task<ApiResult> GetAccountConsent(string consentId, string userTckn, string yosCode, List<string> permissions);

    /// <summary>
    /// Get User's account consent of given account ref by consent id, tckn, yoscode
    /// </summary>
    /// <param name="userTckn">User TCKN number</param>
    /// <param name="consentId"></param>
    /// <param name="yosCode">Yos Code - Bank Code</param>
    /// <param name="accountRef">Account ref</param>
    /// <returns>User's account consent</returns>
    public Task<ApiResult> GetAccountConsentByAccountRef(string consentId, string userTckn, string yosCode, string accountRef);

    /// <summary>
    /// Get user consent by checking id, consentType
    /// Checks consent identity value with given userTCKN.
    /// This metod works for Bireysel consents.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userTckn"></param>
    /// <param name="consentTypes"></param>
    /// <returns>Consent data</returns>
    public Task<ApiResult> GetConsentReadonly(Guid id, string userTckn, List<string> consentTypes);


    /// <summary>
    /// Get active account consents of user - Yetki Bekleniyor, Yetkilendirildi, Yetki Kullanıldı states
    /// Checks identity with consent identity properties
    /// </summary>
    /// <param name="identity">Identity object in account consent</param>
    /// <param name="yosCode">Yos code</param>
    /// <returns>Get active account consents response</returns>
    public Task<ApiResult> GetActiveAccountConsentsOfUser(KimlikDto identity, string yosCode);

    /// <summary>
    /// Getting consent data by checking checksum value in database of account/payment consents
    /// </summary>
    /// <returns>Get previously responsed consent data response</returns>
    public Task<ApiResult> GetIdempotencyRecordOfAccountPaymentConsent(string yosCode,
        string consentType, string checkSumValue);

    /// <summary>
    /// Getting paymentorder data by checking checksum value in database
    /// </summary>
    /// <returns>Get previously responsed payment order record response </returns>
    public Task<ApiResult> GetIdempotencyRecordOfPaymentOrder(string yosCode, string checkSumValue);
}