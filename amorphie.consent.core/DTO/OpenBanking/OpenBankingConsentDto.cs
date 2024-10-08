using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Model;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking;

public class OpenBankingConsentDto : DtoBase
{
    public Guid UserId { get; set; }
    public string State { get; set; }
    public string Description { get; set; }
    public string ConsentType { get; set; }
    public string AdditionalData { get; set; }
    public string UserTCKN { get; set; }
    public string? Scope { get; set; }
    public string? Variant { get; set; }
    public DateTime StateModifiedAt { get; set; }
    public string? StateCancelDetailCode { get; set; }
    public List<TokenDto>? Token { get; set; }
    public virtual ICollection<OBAccountConsentDetailDto> OBAccountConsentDetails { get; set; }
}
