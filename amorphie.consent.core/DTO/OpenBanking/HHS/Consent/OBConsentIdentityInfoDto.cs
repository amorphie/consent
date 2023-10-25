using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class OBConsentIdentityInfoDto : DtoBase
{
    public Guid ConsentId { get; set; }
    public string IdentityType { get; set; }
    public string IdentityData { get; set; }
    public string? InstitutionIdentityType { get; set; }
    public string? InstitutionIdentityData { get; set; }
    public string UserType { get; set; }
}