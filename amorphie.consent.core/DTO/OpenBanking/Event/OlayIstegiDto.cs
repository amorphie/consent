namespace amorphie.consent.core.DTO.OpenBanking.Event;

public class OlayIstegiDto
{
    public KatilimciBilgisiDto katilimciBlg { get; set; } = new();
    public List<OlaylarDto> olaylar { get; set; } = new();
}