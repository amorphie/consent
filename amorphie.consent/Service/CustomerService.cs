using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;

namespace amorphie.consent.Service;

public class CustomerService : ICustomerService
{
    private readonly ICustomerClientService _customerClientService;

    public CustomerService(ICustomerClientService customerClientService)
    {
        _customerClientService = customerClientService;
    }

    public async Task<ApiResult> GetCustomerInformations(KimlikDto kimlik)
    {
        ApiResult result = new();
        try
        {
            var getCustomerRequestDto = new GetCustomerRequestDto
            {
                kmlkTur = kimlik.kmlkTur,
                kmlkVrs = kimlik.kmlkVrs,
                krmKmlkTur = kimlik.krmKmlkTur,
                krmKmlkVrs = kimlik.krmKmlkVrs
            };

            var customerResult = await _customerClientService.GetCustomerInformation(getCustomerRequestDto);

            result.Result = true;
            result.Data = customerResult;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }
}