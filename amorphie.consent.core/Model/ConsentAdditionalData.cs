using amorphie.consent.core.DTO.OpenBanking;

public class AyrikGkd
{
    public string ohkTanimTip { get; set; }
    public string ohkTanimDeger { get; set; }
}

public class Gkd
{
    public string yetYntm { get; set; }
    public string yonAdr { get; set; }
    public AyrikGkd ayrikGkd { get; set; }
    public string hhsYonAdr { get; set; }
    public DateTime yetTmmZmn { get; set; }
}

public class HspBlg
{
    public IznBlg iznBlg { get; set; }
    public object ayrBlg { get; set; }
}

public class IznBlg
{
    public List<string> iznTur { get; set; }
    public DateTime erisimIzniSonTrh { get; set; }
    public DateTime hesapIslemBslZmn { get; set; }
    public DateTime hesapIslemBtsZmn { get; set; }
}

public class KatilimciBlg
{
    public string hhsKod { get; set; }
    public string yosKod { get; set; }
}

public class ConsentAdditionalData
{
    public RzBlg rzBlg { get; set; }
    public KimlikDto kmlk { get; set; }
    public KatilimciBlg katilimciBlg { get; set; }
    public Gkd gkd { get; set; }
    public HspBlg hspBlg { get; set; }
}

public class RzBlg
{
    public string rizaNo { get; set; }
    public DateTime olusZmn { get; set; }
    public DateTime gnclZmn { get; set; }
    public string rizaDrm { get; set; }
    public object rizaIptDtyKod { get; set; }
}

