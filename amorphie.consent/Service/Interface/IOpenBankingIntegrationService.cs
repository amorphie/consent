using amorphie.consent.core.DTO;
using amorphie.consent.core.Model;

namespace amorphie.consent.Service.Interface;

public interface IOpenBankingIntegrationService
{
    Task<ApiResult> AccountList(string customerId, string corporateUserCustomerID);
    Task<ApiResult> VerificationUser(Consent consent);
}