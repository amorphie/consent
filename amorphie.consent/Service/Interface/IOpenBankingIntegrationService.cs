using amorphie.consent.core.DTO;

namespace amorphie.consent.Service.Interface;

public interface IOpenBankingIntegrationService
{
    Task<ApiResult> AccountList(string customerId, string corporateUserCustomerID);
    Task<ApiResult> VerificationUser(string customerId, string corporateUserCustomerID, string jsonData);
}