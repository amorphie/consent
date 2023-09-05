using System.Text.Json.Serialization;
using amorphie.core.Base;

public class HesapHizmetiSağlayici
{
    public IzinBilgisi iznBlg { get; set; }

}

public class GucLuKimlikDogrulama
{
    public string yetYntm { get; set; }
    public string yonAdr { get; set; }
    // public string BldAdr { get; set; }
    public string hhsYonAdr { get; set; }
    public DateTime yetTmmZmn { get; set; }
}

public class hsapRef
{
    public string[] hspRef { get; set; }
}

public class IzinBilgisi
{
    public string[] iznTur { get; set; }
    public string[] hspRef { get; set; }

    public DateTime erisimIzniSonTrh { get; set; }
    public DateTime? hesapIslemBslZmn { get; set; }
    public DateTime? hesapIslemBtsZmn { get; set; }
}

public class HesapBilgisiRizaBilgisi
{
    public string rizaNo { get; set; }
    public DateTime olusZmn { get; set; }
    public DateTime gnclZmn { get; set; }
    public string rizaDrm { get; set; }
    // public string RizaIptDtyKod { get; set; }
}

public class Kimlik
{
    public string kmlkTur { get; set; }
    public string kmlkVrs { get; set; }
    public string ohkTur { get; set; }
}

public class KatilimciBilgisi
{
    public string hhsKod { get; set; }
    public string yosKod { get; set; }
}
public class AyrintiBilgi
{
    public string ohkMsj { get; set; }
}

public class islTtr
{
    public string prBrm { get; set; }
    public string ttr { get; set; }
}

public class alc
{
    public string unv { get; set; }
    public string hspNo { get; set; }
}

public class odmAyr
{
    public string odmKynk { get; set; }
    public string odmAmc { get; set; }
    public object refBlg { get; set; }
    public object odmAcklm { get; set; }
    public object ohkMsj { get; set; }
    public object odmStm { get; set; }
    public object bekOdmZmn { get; set; }
}

public class gon
{
    public string unv { get; set; }
    public string hspNo { get; set; }
    public string hspRef { get; set; }
}

public class odmBsltm
{
    public Kimlik kmlk { get; set; }
    public islTtr islTtr { get; set; }
    public alc alc { get; set; }
    public odmAyr odmAyr { get; set; }
    public gon gon { get; set; }
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
    public Guid id { get; set; }
    public Guid UserId { get; set; }
    public HesapBilgisiRizaBilgisi rzBlg { get; set; }

    public Kimlik kmlk { get; set; }
    public KatilimciBilgisi katilimciBlg { get; set; }
    public GucLuKimlikDogrulama gkd { get; set; }
    public HesapHizmetiSağlayici hspBlg { get; set; }
    public AyrintiBilgi ayrBlg { get; set; }
    public string? Description { get; set; }
    public string xGroupId { get; set; }
}

public class OdemeBilgisiRızaİsteği
{
    public Guid id { get; set; }
    public Guid UserId { get; set; }
    [JsonIgnore]
    public HesapBilgisiRizaBilgisi rzBlg { get; set; }

    public KatilimciBilgisi katilimciBlg { get; set; }
    public GucLuKimlikDogrulama gkd { get; set; }
    public odmBsltm odmBsltm { get; set; }
    public alc alc { get; set; }
    public odmAyr odmAyr { get; set; }
    public gon gon { get; set; }
    public string? Description { get; set; }

}
public class TokenModel : EntityBase
{
    public Guid Id { get; set; }
    public Guid ConsentId { get; set; }
    public string erisimBelirteci { get; set; }
    public int gecerlilikSuresi { get; set; }
    public string yenilemeBelirteci { get; set; }
    public int yenilemeBelirteciGecerlilikSuresi { get; set; }
}
