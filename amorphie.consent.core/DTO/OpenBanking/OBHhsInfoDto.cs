using amorphie.consent.core.DTO.OpenBanking;
using amorphie.core.Base;
namespace amorphie.consent.core.DTO.OpenBanking;

public class OBHhsInfoDto : DtoBase
{
    public string kod { get; set; }
    public string unv { get; set; }
    public string marka { get; set; }
    public string acikAnahtar { get; set; }
    public List<HhsApiBilgiDto> apiBilgileri { get; set; }
    public List<LogoBilgisiDto> logoBilgileri { get; set; }
    public string? ayrikGKD { get; set; }
    public string durum { get; set; }
}
public class HhsApiBilgiDto
{
    public string api { get; set; }
    public string surum { get; set; }
}
