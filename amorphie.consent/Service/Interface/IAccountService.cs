using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;

namespace amorphie.consent.Service.Interface;

public interface IAccountService
{
    /// <summary>
    /// Get accounts of customer from account service
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <returns>Account list of customer</returns>
    Task<ApiResult> GetAccounts(string customerId);

    /// <summary>
    /// Get authorized accounts of customer
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <returns>Authorized account list of customer</returns>
    Task<ApiResult> GetAuthorizedAccounts(string customerId);

    /// <summary>
    /// Get account of customer by account referenece number
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="hspRef">Account Referans Number</param>
    /// <returns>Customer Account of Given Reference</returns>
    Task<ApiResult> GetAccountByHspRef(string customerId, string hspRef);
    /// <summary>
    /// Get account of customer by account referenece number if authorized
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="hspRef">Account Referans Number</param>
    /// <returns>Customer Account of Given Reference if authorized</returns>
    Task<ApiResult> GetAuthorizedAccountByHspRef(string customerId, string hspRef);

    /// <summary>
    /// Get balances of customer
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <returns>Balance list of customer</returns>
    Task<ApiResult> GetBalances(string customerId);

    /// <summary>
    /// Get authorized balances of customer
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <returns>Balance list of customer</returns>
    Task<ApiResult> GetAuthorizedBalances(string customerId);

    /// <summary>
    /// Get balence of customer by account reference number
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="hspRef">Account Referans Number</param>
    /// <returns>Customer Account Balance of Given Account Reference</returns>
    Task<ApiResult> GetBalanceByHspRef(string customerId, string hspRef);

    /// <summary>
    /// Get balence of customer by account reference number if authorized
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="hspRef">Account Referans Number</param>
    /// <returns>Customer Account Balance of Given Account Reference</returns>
    Task<ApiResult> GetAuthorizedBalanceByHspRef(string customerId, string hspRef);

    /// <summary>
    /// Get transactions of account reference number
    /// </summary>
    /// <param name="hspRef">Account Referans Number</param>
    /// <returns>Transactions of Given Account Reference</returns>
    Task<ApiResult> GetTransactionsByHspRef(string hspRef);

}