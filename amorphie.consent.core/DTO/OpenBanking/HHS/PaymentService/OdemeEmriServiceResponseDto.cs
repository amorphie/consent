using amorphie.consent.core.DTO.OpenBanking.PaymentService;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class OdemeEmriServiceResponseDto
{
    public PaymentServiceErrorResponseDto? error { get; set; }
    public RizaBilgileriRequestDto rzBlg { get; set; }
    public KatilimciBilgisiDto katilimciBlg { get; set; }
    public GkdDto gkd { get; set; }
    public OdemeBaslatmaResponseOEDto odmBsltm { get; set; }
    public IsyeriOdemeBilgileriDto? isyOdmBlg { get; set; }
}