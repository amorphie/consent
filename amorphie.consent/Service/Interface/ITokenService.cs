using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;

namespace amorphie.consent.Service.Interface;

public interface ITokenService
{
    /// <summary>
    /// Revoke consent token by consent id 
    /// </summary>
    /// <param name="consentId">Consent id whichs token will be revoked</param>
    /// <returns></returns>
    Task<ApiResult> RevokeConsentToken(Guid consentId);
    
}