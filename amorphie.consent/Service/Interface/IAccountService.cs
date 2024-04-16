using amorphie.consent.core.DTO;
using amorphie.consent.core.Model;

namespace amorphie.consent.Service.Interface;

public interface IAccountService
{
    /// <summary>
    /// Get authorized accounts of customer
    /// Checks user's account consent. If any, get accounts
    /// </summary>
    /// <returns>Authorized account list of customer</returns>
    Task<ApiResult> GetAuthorizedAccounts(HttpContext httpContext, string userTCKN, string consentId, string yosCode,List<OBErrorCodeDetail> errorCodeDetails, int? syfKytSayi, int? syfNo, string? srlmKrtr, string? srlmYon);

    Task<ApiResult> GetAuthorizedAccountsForUI(string userTCKN, List<string> accountReferences,
        int? syfKytSayi, int? syfNo, string? srlmKrtr, string? srlmYon);

    /// <summary>
    /// Get account of customer by account referenece number if authorized
    /// </summary>
    /// <param name="userTCKN">Customer Id</param>
    /// <param name="yosCode"></param>
    /// <param name="hspRef">Account Referans Number</param>
    /// <returns>Customer Account of Given Reference if authorized</returns>
    Task<ApiResult> GetAuthorizedAccountByHspRef(HttpContext httpContext, string userTCKN, string consentId, string yosCode, string hspRef,List<OBErrorCodeDetail> errorCodeDetails);


    /// <summary>
    /// Get authorized balances of customer
    /// </summary>
    /// <returns>Balance list of customer</returns>
    Task<ApiResult> GetAuthorizedBalances(HttpContext httpContext, string userTCKN, string consentId, string yosCode,List<OBErrorCodeDetail> errorCodeDetails, int? syfKytSayi, int? syfNo, string? srlmKrtr, string? srlmYon);


    /// <summary>
    /// Get balence of customer by account reference number if authorized
    /// </summary>
    /// <param name="userTCKN">User tckn</param>
    /// <param name="yosCode">Yos Code</param>
    /// <param name="hspRef">Account Referans Number</param>
    /// <returns>Customer Account Balance of Given Account Reference</returns>
    Task<ApiResult> GetAuthorizedBalanceByHspRef(HttpContext httpContext,string userTCKN, string consentId, string yosCode, string hspRef,List<OBErrorCodeDetail> errorCodeDetails);

    /// <summary>
    /// Get transactions of account reference number
    /// </summary>
    /// <returns>Transactions of Given Account Reference</returns>
    public Task<ApiResult> GetTransactionsByHspRef(HttpContext httpContext, string userTCKN, string consentId, string yosCode,List<OBErrorCodeDetail> errorCodeDetails, string hspRef,
        string psuInitiated,
        DateTime hesapIslemBslTrh,
        DateTime hesapIslemBtsTrh,
        string? minIslTtr,
        string? mksIslTtr,
        string? brcAlc,
        int? syfKytSayi,
        int? syfNo,
        string? srlmKrtr,
        string? srlmYon);

}