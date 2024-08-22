using Refit;

namespace amorphie.consent.Service.Refit;

public interface ICustomerClientService
{

    [Headers("Content-Type: application/json", "user:EBT\\INTERNETUSER")]
    [Post("/customer/openbanking/iscustomer")]
    Task<GetCustomerResponseDto?> GetCustomerInformation(GetCustomerRequestDto requestDto);
}