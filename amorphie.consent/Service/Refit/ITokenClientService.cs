using System.Text.Json.Nodes;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using Refit;

namespace amorphie.consent.Service;

public interface ITokenClientService
{
    [Put("/private/Revoke/ConsentId/{consentId}")]
    Task RevokeConsentToken(Guid consentId);
}