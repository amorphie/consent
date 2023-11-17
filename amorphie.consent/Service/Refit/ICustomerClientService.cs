using System.Text.Json.Nodes;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using Refit;

namespace amorphie.consent.Service.Refit;

/// <summary>
/// Client service to get profile
/// </summary>
public interface ICustomerClientService
{
    [Headers("Authorization:bea19f412375c238f17as81b321e5b1")]
    [Get("/customers/{customerId}/simple-profile")]
    Task<CustomerServiceResponseDto> GetSimpleProfile(string customerId);
    
}