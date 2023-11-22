using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking;

public class OpenBankingConsentDto : DtoBase
{
    public Guid UserId { get; set; }
    public string State { get; set; }
    public string Description { get; set; }
    public string ConsentType { get; set; }
    public string AdditionalData { get; set; }
    public List<TokenDto>? Token { get; set; }
}
