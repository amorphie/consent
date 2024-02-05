using System.Text.Json.Nodes;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using Refit;

namespace amorphie.consent.Service.Refit;

public interface IAccountClientService
{
    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar?customerId[customerId]={customerId}")]
    Task<bool> IsCustomer(string customerId);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}?syfKytSayi={syfKytSayi}&syfNo={syfNo}&srlmKrtr={srlmKrtr}&srlmYon={srlmYon}")]
    Task<List<HesapBilgileriDto>> GetAccounts(string customerId, 
        int? syfKytSayi =OpenBankingConstants.AccountServiceParameters.syfKytSayi,
        int? syfNo = OpenBankingConstants.AccountServiceParameters.syfNo,
        string? srlmKrtr = OpenBankingConstants.AccountServiceParameters.srlmKrtr,
        string? srlmYon = OpenBankingConstants.AccountServiceParameters.srlmYon);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/{hspRef}")]
    Task<HesapBilgileriDto?> GetAccountByHspRef(string customerId, string hspRef);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/bakiye?syfKytSayi=5&syfNo=1&srlmKrtr=hspRef&srlmYon=A")]
    Task<List<BakiyeBilgileriDto>> GetBalances(string customerId);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/bakiye/{hspRef}")]
    Task<BakiyeBilgileriDto?> GetBalanceByHspRef(string customerId, string hspRef);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{hspRef}/islemler?hesapIslemBslTrh=2019-04-20&hesapIslemBtsTrh=2019-05-17&minIslTtr=100&mksIslTtr=500000&brcAlc=B&syfKytSayi=50&syfNo=1&srlmKrtr=islGrckZaman&srlmYon=Y")]
    Task<IslemBilgileriDto?> GetTransactionsByHspRef(string hspRef);
}