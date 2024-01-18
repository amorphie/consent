using amorphie.consent.core.DTO.OpenBanking;
using amorphie.core.Base;
namespace amorphie.consent.core.Model;

public class OBYosInfo : EntityBase
{
    public string Kod { get; set; }
    public string Unv { get; set; }
    public string Marka { get; set; }
    public string AcikAnahtar { get; set; }
    public List<string> Roller { get; set; }
    public string Adresler { get; set; }
    public string LogoBilgileri { get; set; }
    public string ApiBilgileri { get; set; }
    public string Durum { get; set; }

}