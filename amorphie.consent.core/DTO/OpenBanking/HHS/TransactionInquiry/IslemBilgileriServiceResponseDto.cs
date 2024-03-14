namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class IslemBilgileriServiceResponseDto
{
    public string hspRef { get; set; } = string.Empty;
    public List<IslemDto>? isller { get; set; }
    public int toplamIslemSayisi { get; set; }
}
