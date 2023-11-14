namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class OdemeEmriServiceResponseDto
{
    public string error { get; set; }
    public RizaBilgileriRequestDto rzBlg { get; set; }
    public KatilimciBilgisiDto katilimciBlg { get; set; }
    public GkdDto gkd { get; set; }
    public OdemeBaslatmaResponseOEDto odmBsltm { get; set; }
    public IsyeriOdemeBilgileriDto? isyOdmBlg { get; set; }
}