using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;

namespace amorphie.consent.Service.Interface;

public interface ICustomerService
{
    /// <summary>
    /// Get simple profile of given Identity number customer
    /// </summary>
    /// <param name="customerId">Customer Id</param>
    /// <returns>Simple profile of customer</returns>
    Task<ApiResult> GetSimpleProfile(string customerId);
   
}