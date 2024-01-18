using amorphie.consent.core.DTO.OpenBanking;
using amorphie.core.Base;
namespace amorphie.consent.core.DTO.OpenBanking;

public class OBHhsInfoDto : DtoBase
{
    public string kod { get; set; }
    public string unv { get; set; }
    public string marka { get; set; }
    public string acikAnahtar { get; set; }
    public List<HhsApiBilgi> apiBilgileri { get; set; }
    public List<LogoBilgisi> logoBilgileri { get; set; }
    public string? ayrikGKD { get; set; }
    public string durum { get; set; }
}
public class HhsApiBilgi
{
    public string Api { get; set; }
    public string Surum { get; set; }
}
