using amorphie.core.Base;
namespace amorphie.consent.core.DTO.OpenBanking;

public class OBYosInfoDto : DtoBase
{

    public string kod { get; set; }
    public string unv { get; set; }
    public string marka { get; set; }
    public string acikAnahtar { get; set; }
    public List<string> roller { get; set; }
    public List<Adres> adresler { get; set; }
    public List<LogoBilgisi> logoBilgileri { get; set; }
    public List<YosApiBilgi> apiBilgileri { get; set; }
    public string durum { get; set; }

}

public class AdresDetayi
{
    public string TmlAdr { get; set; }
    public string Aciklama { get; set; }
}

public class Adres
{
    public string YetYntm { get; set; }
    public List<AdresDetayi> AdresDetaylari { get; set; }
}

public class LogoBilgisi
{
    public string LogoTur { get; set; }
    public string LogoAdr { get; set; }
    public string LogoArkaPlan { get; set; }
    public string LogoFormat { get; set; }
}

public class YosApiBilgi
{
    public string Api { get; set; }
    public string Surum { get; set; }
}