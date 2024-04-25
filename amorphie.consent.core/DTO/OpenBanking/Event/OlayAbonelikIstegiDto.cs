namespace amorphie.consent.core.DTO.OpenBanking.Event;

public class OlayAbonelikIstegiDto
{
    public KatilimciBilgisiDto katilimciBlg { get; set; } = new();
    public List<AbonelikTipleriDto> abonelikTipleri { get; set; } = new();
}