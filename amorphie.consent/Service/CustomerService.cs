using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using Microsoft.EntityFrameworkCore;

namespace amorphie.consent.Service;

public class CustomerService : ICustomerService
{
    private readonly ICustomerClientService _customerClientService;
    private readonly ConsentDbContext _context;

    public CustomerService(ICustomerClientService customerClientService,
        ConsentDbContext context)
    {
        _customerClientService = customerClientService;
        _context = context;
    }

    public async Task<ApiResult> GetSimpleProfile(string customerId)
    {
        ApiResult result = new();
        try
        {
            //Get simple profile of customer from service
            result.Data = await _customerClientService.GetSimpleProfile(customerId);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }
}