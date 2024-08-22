namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class CustomerScanResponseDto
{
    public string? customernumber { get; set; }
    public string? gsmno { get; set; }
    public string? tckn { get; set; }
    public bool isUniqueUserFound { get; set; }
    public CustomerScanErrorResponseDto? error { get; set; }
}