using System.Text.Json.Serialization;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class HesapBilgisiRizaIstegiHHSDto
{
    public HesapBilgisiRizaIstegiHHSDto()
    {
        katilimciBlg = new KatilimciBilgisiDto();
        gkd = new GkdRequestDto();
        kmlk = new KimlikDto();
        hspBlg = new HesapBilgisiRequestDto();
    }

    public KatilimciBilgisiDto katilimciBlg { get; set; }
    public GkdRequestDto gkd { get; set; }
    public KimlikDto kmlk { get; set; }
    public HesapBilgisiRequestDto hspBlg { get; set; }
}