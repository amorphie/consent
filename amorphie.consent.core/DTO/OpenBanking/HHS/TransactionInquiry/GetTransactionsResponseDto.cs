namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class GetTransactionsResponseDto
{
    public AccountServiceErrorResponseDto? error { get; set; }
    public ListIslemBilgileriDto? data { get; set; }
}
