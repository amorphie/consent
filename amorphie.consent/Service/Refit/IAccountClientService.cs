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
    [Post("/hesaplar/{customerId}?syfKytSayi={syfKytSayi}&syfNo={syfNo}&srlmKrtr={srlmKrtr}&srlmYon={srlmYon}")]
    Task<ListHesapBilgileriDto?> GetAccounts( [Header("izinTur")] string izinTur,
        [Body] GetHesapBilgileriRequestDto accountRefs,
        string customerId,
        int syfKytSayi,
        int syfNo,
        string srlmKrtr,
        string srlmYon);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/{hspRef}")]
    Task<HesapBilgileriDto?> GetAccountByHspRef(string customerId, string hspRef);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/bakiye?syfKytSayi={syfKytSayi}&syfNo={syfNo}&srlmKrtr={srlmKrtr}&srlmYon={srlmYon}")]
    Task<ListBakiyeBilgileriDto?> GetBalances(string customerId,
        int syfKytSayi,
        int syfNo,
        string srlmKrtr,
        string srlmYon);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/bakiye/{hspRef}")]
    Task<BakiyeBilgileriDto?> GetBalanceByHspRef(string customerId, string hspRef);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{hspRef}/islemler")]
    Task<IslemBilgileriServiceResponseDto?> GetTransactionsByHspRef(
        string hspRef,
        string hesapIslemBslTrh,
        string hesapIslemBtsTrh,
        [AliasAs("syfKytSayi")] int syfKytSayi,
        [AliasAs("syfNo")] int syfNo,
        [AliasAs("srlmKrtr")] string srlmKrtr,
        [AliasAs("srlmYon")] string srlmYon,
        [AliasAs("minIslTtr")] string? minIslTtr,
        [AliasAs("mksIslTtr")] string? mksIslTtr,
        [AliasAs("brcAlc")] string? brcAlc,
        [Header("izinTur")] string permissionType,
        [Header("ohkTur")] string ohkTur,
        [Header("PSU-Initiated")] string psuInitiated);


}