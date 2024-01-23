using amorphie.core.Base;
namespace amorphie.consent.core.DTO.OpenBanking;

public class OBYosInfoDto : DtoBase
{

    public string kod { get; set; }
    public string unv { get; set; }
    public string marka { get; set; }
    public string acikAnahtar { get; set; }
    public List<string> roller { get; set; }
    public List<AdresDto> adresler { get; set; }
    public List<LogoBilgisiDto> logoBilgileri { get; set; }
    public List<YosApiBilgiDto> apiBilgileri { get; set; }
    public string durum { get; set; }

}

public class AdresDetayiDto
{
    public string tmlAdr { get; set; }
    public string aciklama { get; set; }
}

public class AdresDto
{
    public string yetYntm { get; set; }
    public List<AdresDetayiDto> adresDetaylari { get; set; }
}

public class LogoBilgisiDto
{
    public string logoTur { get; set; }
    public string logoAdr { get; set; }
    public string logoArkaPlan { get; set; }
    public string logoFormat { get; set; }
}

public class YosApiBilgiDto
{
    public string api { get; set; }
    public string surum { get; set; }
}