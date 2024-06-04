using Refit;

namespace amorphie.consent.Service.Refit;

public interface ICustomerClientService
{

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Post("/hesaplar/{customerId}?syfKytSayi={syfKytSayi}&syfNo={syfNo}&srlmKrtr={srlmKrtr}&srlmYon={srlmYon}")]
    Task<GetCustomerResponseDto?> GetCustomerInformation(GetCustomerRequestDto requestDto);
}