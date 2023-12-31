using amorphie.core.Base;
namespace amorphie.consent.core.DTO.OpenBanking;

public class OBYosInfoDto : DtoBase
{
    public List<string> roller { get; set; }
    public string kod { get; set; }
    public string unv { get; set; }
    public string marka { get; set; }
    public string acikAnahtar { get; set; }
    public List<string> adresler { get; set; }
    public List<string> logoBilgileri { get; set; }
}
