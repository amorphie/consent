namespace amorphie.consent.core.DTO.OpenBanking.Event;

public class OlayAbonelikDto
{
    public string olayAbonelikNo { get; set; }
    public DateTime olusturmaZamani { get; set; }
    public DateTime? guncellemeZamani { get; set; }
    public KatilimciBilgisiDto katilimciBlg { get; set; } = new();
    public List<AbonelikTipleriDto> abonelikTipleri { get; set; } = new List<AbonelikTipleriDto>();
}