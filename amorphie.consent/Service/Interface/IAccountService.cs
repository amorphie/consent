using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;

namespace amorphie.consent.Service.Interface;

public interface IAccountService
{
    /// <summary>
    /// Get accounts of customer
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <returns>Account list of customer</returns>
    Task<ApiResult> GetAccounts(string customerId);

    /// <summary>
    /// Get accounts of customer
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <param name="hspRef">Hesap Referans Number</param>
    /// <returns>Customer Account of Given Referens</returns>
    Task<ApiResult> GetAccountByHspRef(string customerId, string hspRef);
}