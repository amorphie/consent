using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.Enum;
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

            var customerInformation = await _customerClientService.GetCustomerInformation(getCustomerRequestDto);
            if (customerInformation is null)//Customer information could not be taken
            {
                result.Result = false;
            }
            else if (customerInformation.isCustomer 
                          && (string.IsNullOrEmpty(customerInformation.citizenshipNumber) 
                              || string.IsNullOrEmpty(customerInformation.customerNumber)))
            {//Although valid customer, required fields empty
                result.Result = false;
            }
            else if (customerInformation.krmIsCustomer 
                     && kimlik.ohkTur == OpenBankingConstants.OHKTur.Kurumsal
                     && (string.IsNullOrEmpty(customerInformation.krmCustomerNumber) 
                         || !customerInformation.isCustomer))
            {//Valid institution customer, required fields empty
                result.Result = false;
            }
            
            result.Data = customerInformation;
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }

        return result;
    }
}