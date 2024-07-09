using amorphie.consent.core.DTO;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.Helper;
using amorphie.consent.Service.Interface;
using Microsoft.AspNetCore.Mvc;

public class OpenBankingIntegrationService : IOpenBankingIntegrationService
{
    private readonly string _url;
    private readonly string _username;
    private readonly string _password;
    private readonly IConfiguration _configuration; 
    private readonly ILogger<OpenBankingIntegrationService> _logger;

    public OpenBankingIntegrationService(IConfiguration configuration,
        ILogger<OpenBankingIntegrationService> logger
        )
    {
        _configuration = configuration;
        _url = configuration["OB_Integration_Url"];
        _username = configuration["OB_Integration_Username"];
        _password = configuration["OB_Integration_Password"];
        _logger = logger;
    }
    public async Task<ApiResult> AccountList(string customerId, string corporateUserCustomerID)
    {
        ApiResult result = new();
        try
        {
            
            
            var client = new OpenBankingIntegrationClient();
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_url);
            var serviceResult = await client.AccountListAsync(null, _username, _password, customerId, corporateUserCustomerID);

            result.Result = true;
            result.Data = serviceResult;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }

    public async Task<ApiResult> VerificationUser(Consent consent)
    {
        ApiResult result = new();
        try
        {
            
            var verificationUserJsonData = new VerificationUserJsonData
            {
                account = Array.Empty<string>()
            };

            string customerNumber, krmCustomerNumber;
            if (consent.ConsentType == ConsentConstants.ConsentType.OpenBankingAccount)
            {//Account consent
                var accountConsent = consent.OBAccountConsentDetails.FirstOrDefault();
                if (accountConsent is null)
                {
                    result.Result = false;
                    result.Message = "accountConsent is null.";
                    return result;
                }
                if (accountConsent.AccountReferences != null)
                    verificationUserJsonData.account = accountConsent.AccountReferences.ToArray();
                customerNumber = accountConsent.CustomerNumber!;
                krmCustomerNumber = accountConsent.InstitutionCustomerNumber!;
            }
            else
            {//Payment consent
                var paymentConsent = consent.OBPaymentConsentDetails.FirstOrDefault();
                if (paymentConsent is null)
                {
                    result.Result = false;
                    result.Message = "paymentConsent is null";
                    return result;
                }
                customerNumber = paymentConsent.CustomerNumber!;
                krmCustomerNumber = paymentConsent.InstitutionCustomerNumber!;
            }
            
            var client = new OpenBankingIntegrationClient();
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_url);

            var serviceResult =
                    await client.VerificationUserAsync(null, _username, _password, customerNumber, krmCustomerNumber, verificationUserJsonData.ToJsonString());
            
            result.Result = true;
            result.Data = serviceResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in VerificationUser");
            result.Result = false;
            result.Message = $"Error in VerificationUser. {e.Message}";
        }

        return result;
    }
}