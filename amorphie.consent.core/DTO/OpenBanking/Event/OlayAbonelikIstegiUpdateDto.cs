namespace amorphie.consent.core.DTO.OpenBanking.Event;

public class OlayAbonelikIstegiUpdateDto
{
    public string olayAbonelikNo { get; set; }
    public KatilimciBilgisiDto katilimciBlg { get; set; } = new();
    public List<AbonelikTipleriDto> abonelikTipleri { get; set; } = new();
}