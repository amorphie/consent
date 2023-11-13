namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class OdemeEmriRizasiServiceResponseDto
{
    public string error { get; set; }
    public RizaBilgileriDto rzBlg { get; set; }
    public KatilimciBilgisiDto katilimciBlg { get; set; }
    public GkdRequestDto gkd { get; set; }
    public OdemeBaslatmaDto odmBsltm { get; set; }
    public IsyeriOdemeBilgileriDto? isyOdmBlg { get; set; }
}