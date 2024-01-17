namespace amorphie.consent.core.DTO.OpenBanking.Event;

public class OlayIstegiDto
{
    public KatilimciBilgisiDto katilimciBlg { get; set; }
    public List<OlaylarDto> olaylar { get; set; } = new List<OlaylarDto>();
}