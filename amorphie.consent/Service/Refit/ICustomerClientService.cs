using System.Text.Json.Nodes;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using Refit;

namespace amorphie.consent.Service.Refit;

public interface ICustomerClientService
{

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Post("/hesaplar/{customerId}?syfKytSayi={syfKytSayi}&syfNo={syfNo}&srlmKrtr={srlmKrtr}&srlmYon={srlmYon}")]
    Task<GetAccountsResponseDto?> GetCustomerInformation([Header("izinTur")] string izinTur,
        [Body] GetByAccountRefRequestDto accountRefs,
        string customerId,
        int syfKytSayi,
        int syfNo,
        string srlmKrtr,
        string srlmYon);

}