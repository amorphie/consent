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
    [Get("/hesaplar/{customerId}?syfKytSayi={syfKytSayi}&syfNo={syfNo}&srlmKrtr={srlmKrtr}&srlmYon={srlmYon}&izinTur={izinTur}")]
    Task<ListHesapBilgileriDto?> GetAccounts(string customerId,
        string izinTur,
        int syfKytSayi = OpenBankingConstants.AccountServiceParameters.syfKytSayi,
        int syfNo = OpenBankingConstants.AccountServiceParameters.syfNo,
        string srlmKrtr = OpenBankingConstants.AccountServiceParameters.srlmKrtrAccount,
        string srlmYon = OpenBankingConstants.AccountServiceParameters.srlmYon);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/{hspRef}")]
    Task<HesapBilgileriDto?> GetAccountByHspRef(string customerId, string hspRef);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/bakiye?syfKytSayi={syfKytSayi}&syfNo={syfNo}&srlmKrtr={srlmKrtr}&srlmYon={srlmYon}")]
    Task<ListBakiyeBilgileriDto?> GetBalances(string customerId,
        int syfKytSayi = OpenBankingConstants.AccountServiceParameters.syfKytSayi,
        int syfNo = OpenBankingConstants.AccountServiceParameters.syfNo,
        string srlmKrtr = OpenBankingConstants.AccountServiceParameters.srlmKrtrAccount,
        string srlmYon = OpenBankingConstants.AccountServiceParameters.srlmYon);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/bakiye/{hspRef}")]
    Task<BakiyeBilgileriDto?> GetBalanceByHspRef(string customerId, string hspRef);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{hspRef}/islemler?hesapIslemBslTrh={hesapIslemBslTrh}&hesapIslemBtsTrh={hesapIslemBtsTrh}&syfKytSayi={syfKytSayi}&syfNo={syfNo}&srlmKrtr={srlmKrtr}&srlmYon={srlmYon}&izinTur={izinTur}")]
    Task<IslemBilgileriDto?> GetTransactionsByHspRef(string hspRef,
        DateTime hesapIslemBslTrh,
        DateTime hesapIslemBtsTrh,
        string izinTur,
        int syfKytSayi = OpenBankingConstants.AccountServiceParameters.syfKytSayi,
        int syfNo = OpenBankingConstants.AccountServiceParameters.syfNo,
        string srlmKrtr = OpenBankingConstants.AccountServiceParameters.srlmKrtrAccount,
        string srlmYon = OpenBankingConstants.AccountServiceParameters.srlmYon,
        [Header("minIslTtr")] string minIslTtr = null,
        [Header("mksIslTtr")] string mksIslTtr = null,
        [Header("brcAlc")] string brcAlc = null);
}