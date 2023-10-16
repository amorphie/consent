using System.Text.Json.Nodes;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using Refit;

namespace amorphie.consent.Service;

public interface IAccountClientService
{
    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar?customerId[customerId]={customerId}")]
    Task<bool> IsCustomer(string customerId);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}?syfKytSayi=5&syfNo=1&srlmKrtr=hspRef&srlmYon=A")]
    Task<List<HesapBilgileriDto>> GetAccounts(string customerId);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/{hspRef}")]
    Task<HesapBilgileriDto?> GetAccountByHspRef(string customerId, string hspRef);
    
    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/bakiye?syfKytSayi=5&syfNo=1&srlmKrtr=hspRef&srlmYon=A")]
    Task<List<BakiyeBilgileriDto>> GetBalances(string customerId);

    [Headers("Content-Type: application/json", "CHANNEL:INTERNET", "branch:2000", "user:EBT\\INTERNETUSER")]
    [Get("/hesaplar/{customerId}/bakiye/{hspRef}")]
    Task<BakiyeBilgileriDto?> GetBalanceByHspRef(string customerId, string hspRef);
}