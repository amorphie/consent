using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;

namespace amorphie.consent.Service;

public class AccountService : IAccountService
{
    private readonly IAccountClientService _accountClientService;

    public AccountService(IAccountClientService accountClientService)
    {
        _accountClientService = accountClientService;
    }

    public async Task<ApiResult> GetAccounts(string customerId)
    {
        ApiResult result = new();
        try
        {
            //Get accounts of customer from service
            result.Data = await _accountClientService.GetAccounts(customerId);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }

    public async Task<ApiResult> GetAccountByHspRef(string customerId, string hspRef)
    {
        ApiResult result = new();
        try
        {
            //Get account of customer from service
            result.Data = await _accountClientService.GetAccountByHspRef(customerId, hspRef);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }
}