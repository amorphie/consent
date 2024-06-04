using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;

namespace amorphie.consent.Service.Interface;

public interface ICustomerService
{
   
    /// <summary>
    /// Get customer informations
    /// </summary>
    Task<ApiResult> GetCustomerInformations(KimlikDto kimlik);
}