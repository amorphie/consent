using amorphie.consent.core.DTO;
using amorphie.consent.core.Model;

namespace amorphie.consent.Service.Interface;

public interface ICustomerService
{
   
    /// <summary>
    /// Get customer informations
    /// </summary>
    Task<ApiResult> GetCustomerInformations(HttpContext httpContext, string userTCKN, string consentId, string yosCode,List<OBErrorCodeDetail> errorCodeDetails, int? syfKytSayi, int? syfNo, string? srlmKrtr, string? srlmYon);


}