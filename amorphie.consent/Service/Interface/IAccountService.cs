using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;

namespace amorphie.consent.Service.Interface;

public interface IAccountService
{
    /// <summary>
    /// Get authorized accounts of customer
    /// Checks user's account consent. If any, get accounts
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="userTCKN">User TCKN</param>
    /// <param name="yosCode"></param>
    /// <param name="syfKytSayi">Page size</param>
    /// <param name="syfNo">Page number</param>
    /// <param name="srlmKrtr">Order by name</param>
    /// <param name="srlmYon">Order by direction</param>
    /// <returns>Authorized account list of customer</returns>
    Task<ApiResult> GetAuthorizedAccounts(HttpContext httpContext, string userTCKN, string yosCode, int? syfKytSayi, int? syfNo, string? srlmKrtr, string? srlmYon);

    /// <summary>
    /// Get account of customer by account referenece number if authorized
    /// </summary>
    /// <param name="userTCKN">Customer Id</param>
    /// <param name="yosCode"></param>
    /// <param name="hspRef">Account Referans Number</param>
    /// <returns>Customer Account of Given Reference if authorized</returns>
    Task<ApiResult> GetAuthorizedAccountByHspRef(string userTCKN, string yosCode, string hspRef);


    /// <summary>
    /// Get authorized balances of customer
    /// </summary>
    /// <param name="userTCKN">User tckn</param>
    /// <param name="yosCode">Yos Code</param>
    /// <param name="syfKytSayi">Page size</param>
    /// <param name="syfNo">Page number</param>
    /// <param name="srlmKrtr">Order by name</param>
    /// <param name="srlmYon">Order by direction</param>
    /// <returns>Balance list of customer</returns>
    Task<ApiResult> GetAuthorizedBalances(string userTCKN, string yosCode, int? syfKytSayi, int? syfNo, string? srlmKrtr, string? srlmYon);


    /// <summary>
    /// Get balence of customer by account reference number if authorized
    /// </summary>
    /// <param name="userTCKN">User tckn</param>
    /// <param name="yosCode">Yos Code</param>
    /// <param name="hspRef">Account Referans Number</param>
    /// <returns>Customer Account Balance of Given Account Reference</returns>
    Task<ApiResult> GetAuthorizedBalanceByHspRef(string userTCKN, string yosCode, string hspRef);

    /// <summary>
    /// Get transactions of account reference number
    /// </summary>
    /// <param name="hspRef">Account Referans Number</param>
    /// <returns>Transactions of Given Account Reference</returns>
    public Task<ApiResult> GetTransactionsByHspRef(string userTCKN, string yosCode, string hspRef,
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