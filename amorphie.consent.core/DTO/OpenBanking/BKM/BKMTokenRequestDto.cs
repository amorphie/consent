using Refit;
namespace amorphie.consent.core.DTO.OpenBanking;

public class BKMTokenRequestDto
{
    [AliasAs("client_id")]
    public string ClientId { get; set; }

    [AliasAs("client_secret")]
    public string ClientSecret { get; set; }

    [AliasAs("grant_type")]
    public string GrantType { get; set; }

    [AliasAs("scope")]
    public string Scope { get; set; }
}