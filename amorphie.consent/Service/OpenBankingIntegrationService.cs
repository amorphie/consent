using amorphie.consent.core.DTO;
using amorphie.consent.Service.Interface;
using Microsoft.AspNetCore.Mvc;

public class OpenBankingIntegrationService : IOpenBankingIntegrationService
{
    private readonly string _url;
    private readonly string _username;
    private readonly string _password;

    public OpenBankingIntegrationService
        (
            [FromServices] IConfiguration configuration
        )
    {
        _url = configuration["OB_Integration_Url"];
        _username = configuration["OB_Integration_Username"];
        _password = configuration["OB_Integration_Password"];
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

    public async Task<ApiResult> VerificationUser(string customerId, string corporateUserCustomerID, string jsonData)
    {
        ApiResult result = new();
        try
        {
            var client = new OpenBankingIntegrationClient();

            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_url);

            var serviceResult =
                    await client.VerificationUserAsync(null, _username, _password, customerId, corporateUserCustomerID, jsonData);

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
}