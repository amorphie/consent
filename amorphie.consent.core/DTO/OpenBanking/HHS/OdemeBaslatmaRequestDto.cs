
namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class OdemeBaslatmaRequestDto
{
    public KimlikDto? kmlk { get; set; }
    public TutarDto islTtr { get; set; }
    public GonderenHesapDto? gon { get; set; }
    public AliciHesapRequestDto alc { get; set; }
    public KarekodDto? kkod { get; set; }
    public OdemeAyrintilariRequestDto odmAyr { get; set; }

}
