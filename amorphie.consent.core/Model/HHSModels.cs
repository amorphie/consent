using amorphie.core.Base;

public class HesapHizmetiSağlayici
{
    public IzinBilgisi iznBlg { get; set; }

}

public class GucLuKimlikDogrulama
{
    public string YetYntm { get; set; }
    public string YonAdr { get; set; }
    // public string BldAdr { get; set; }
    public string HhsYonAdr { get; set; }
    public DateTime YetTmmZmn { get; set; }
}

public class IzinBilgisi
{
    public string[] IzinTur { get; set; }
    public DateTime ErisimIzniSonTrh { get; set; }
    public DateTime? HesapIslemBslZmn { get; set; }
    public DateTime? HesapIslemBtsZmn { get; set; }
}

public class HesapBilgisiRizaBilgisi
{
    public string RizaNo { get; set; }
    public DateTime OlusZmn { get; set; }
    public DateTime GnclZmn { get; set; }
    public string RizaDrm { get; set; }
    // public string RizaIptDtyKod { get; set; }
}

public class Kimlik
{
    public string KimlikTur { get; set; }
    public string KimlikVrs { get; set; }
    public string ohkTur { get; set; }
}

public class KatilimciBilgisi
{
    public string HhsKod { get; set; }
    public string YosKod { get; set; }
}
public class AyrintiBilgi
{
    public string OhkMsj { get; set; }
}

public class HesapBilgisiRizaIstegi
{
    public HesapBilgisiRizaBilgisi RzBlg { get; set; }
    public Kimlik Kmlk { get; set; }
    public KatilimciBilgisi KatilimciBlg { get; set; }
    public GucLuKimlikDogrulama Gkd { get; set; }
    public HesapHizmetiSağlayici HspBlg { get; set; }
    public IzinBilgisi IznBlg { get; set; }
}

public class HesapBilgisiRizaIstegiResponse
{
    public HesapBilgisiRizaBilgisi RzBlg { get; set; }
    public Kimlik Kmlk { get; set; }
    public KatilimciBilgisi KatilimciBlg { get; set; }
    public GucLuKimlikDogrulama Gkd { get; set; }
    public HesapHizmetiSağlayici HspBlg { get; set; }
    public AyrintiBilgi AyrintiBlg { get; set; }
}

public class TokenModel
{
    public Guid Id { get; set; }
    public string erisimBelirteci { get; set; }
    public int gecerlilikSuresi { get; set; }
    public string yenilemeBelirteci { get; set; }
    public int yenilemeBelirteciGecerlilikSuresi { get; set; }
}

// public class HesapBilgisiRizaBilgisiDto
// {
//     public string RizaNo { get; set; }
//     public DateTime OlusZmn { get; set; }
//     public DateTime GnclZmn { get; set; }
//     public string RizaDrm { get; set; }
//     public string RizaIptDtyKod { get; set; }
// }

// public class KimlikDto
// {
//     public string KimlikTur { get; set; }
//     public string KimlikVrs { get; set; }
//     public string KrmKmlkTur { get; set; }
//     public string KrmKmlkVrs { get; set; }
//     public string OhkTur { get; set; }
// }

// public class KatilimciBilgisiDto
// {
//     public string HhsKod { get; set; }
//     public string YosKod { get; set; }
// }

// public class GucLuKimlikDogrulamaDto
// {
//     public string YetYntm { get; set; }
//     public string YonAdr { get; set; }
//     public string BldAdr { get; set; }
//     public string HhsYonAdr { get; set; }
//     public DateTime YetTmmZmn { get; set; }
// }

// public class IzinBilgisiDto
// {
//     public string[] IzinTur { get; set; }
//     public DateTime ErisimIzniSonTrh { get; set; }
//     public DateTime? HesapIslemBslZmn { get; set; }
//     public DateTime? HesapIslemBtsZmn { get; set; }
// }
