using System.Text.Json.Serialization;
using amorphie.core.Base;

public class HesapBilgisi
{
    public IzinBilgisi iznBlg { get; set; }
    public AyrintiBilgi ayrBlg { get; set; }
    public string ohkMsj { get; set; }

}

public class Gkd
{
    public string yetYntm { get; set; }
    public string yonAdr { get; set; }
    public string BldAdr { get; set; }
    public string hhsYonAdr { get; set; }
    public DateTime yetTmmZmn { get; set; }
}

public class hsapRef{
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

public class RizaBilgileri
{
    public string rizaNo { get; set; }
    public DateTime olusZmn { get; set; }
    public DateTime gnclZmn { get; set; }
    public string rizaDrm { get; set; }
    public string RizaIptDtyKod { get; set; }
}

public class Kimlik
{
    public string kmlkTur { get; set; }
    public string kmlkVrs { get; set; }
    public string krmKmlkTur { get; set; }
    public string krmKmlkVrs { get; set; }
    public string ohkTur { get; set; }
}

public class KatilimciBilgisi
{
    public string hhsKod { get; set; }
    public string yosKod { get; set; }
}
public class AyrintiBilgi
{
    //TODO obje detayı alınacak.
    // public string ohkMsj { get; set; }
}

public class Tutar
{
    public string prBrm { get; set; }
    public string ttr { get; set; }
}

public class AlcHesap
{
    public string unv { get; set; }
    public string hspNo { get; set; }
    public Kolas kolas { get; set; }
}

public class OdemeAyrintilari
{
    public string odmKynk { get; set; }
    public string odmAmc { get; set; }
    public string refBlg { get; set; }
    public string odmAcklm { get; set; }
    public string ohkMsj { get; set; }
    public string odmStm { get; set; }
    public DateTime bekOdmZmn { get; set; }
}

public class GonHesap
{
    public string unv { get; set; }
    public string hspNo { get; set; }
    public string hspRef { get; set; }
}

public class OdemeBaslatma
{
    public Kimlik kmlk { get; set; }
    public Tutar islTtr { get; set; }
    public AlcHesap alc { get; set; }
    public OdemeAyrintilari odmAyr { get; set; }
    public Karekod? kkod { get; set; }
    public hhsMsrfTtr? hhsMsrfTtr { get; set; }
    public obhsMsrfTtr? obhsMsrfTtr { get; set; }
    public GonHesap gon { get; set; }
}

public class Kolas{
    public string kolasTur { get; set; }
    public string kolasDgr { get; set; }
    public string kolasRefNo { get; set; }
    public string kolasHspTur { get; set; }
}

public class Karekod{
    public string aksTur { get; set; }
    public string kkodRef { get; set; }
    public string kkodUrtcKod { get; set; }

}

public class obhsMsrfTtr{
    public string prBrm { get; set; }
    public string ttr { get; set; }
}
public class hhsMsrfTtr{
    public string prBrm { get; set; }
    public string ttr { get; set; }
}
public class IsyeriOdemeBilgileri{
    public string isyKtgKod { get; set; }
    public string altIsyKtgKod { get; set; }
    public string genelUyeIsyeriNo { get; set; }
}


public class HesapBilgisiRizaIstegi
{
    public RizaBilgileri RzBlg { get; set; }
    public Kimlik Kmlk { get; set; }
    public KatilimciBilgisi KatilimciBlg { get; set; }
    public Gkd Gkd { get; set; }
    public HesapBilgisi HspBlg { get; set; }
    public IzinBilgisi IznBlg { get; set; }
}

public class HesapBilgisiRizaIstegiResponse
{
    public Guid id { get; set; }
    public Guid UserId { get; set; }
    public RizaBilgileri rzBlg { get; set; }

    public Kimlik kmlk { get; set; }
    public KatilimciBilgisi katilimciBlg { get; set; }
    public Gkd gkd { get; set; }
    public HesapBilgisi hspBlg { get; set; }
    public string? Description { get; set; }
    public string xGroupId { get; set; }
}

public class  OdemeBilgisiRızaİsteği{
    public Guid id { get; set; }
    public Guid UserId { get; set; }
    [JsonIgnore]
    public RizaBilgileri rzBlg { get; set; }

    public KatilimciBilgisi katilimciBlg { get; set; }
    public Gkd gkd { get; set; }
    public OdemeBaslatma odmBsltm { get; set; }
    public IsyeriOdemeBilgileri isyOdmBlg { get; set; }
    public string? Description { get; set; }

}
public class TokenModel:EntityBase
{
    public Guid Id { get; set; }
    public Guid ConsentId { get; set; }
    public string erisimBelirteci { get; set; }
    public int gecerlilikSuresi { get; set; }
    public string yenilemeBelirteci { get; set; }
    public int yenilemeBelirteciGecerlilikSuresi { get; set; }
}
