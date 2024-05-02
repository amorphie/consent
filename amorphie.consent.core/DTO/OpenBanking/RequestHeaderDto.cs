using Microsoft.AspNetCore.Mvc;

namespace amorphie.consent.core.DTO.OpenBanking;
public class RequestHeaderDto
{
    public string XRequestID { get; set; } = string.Empty;
    public string XGroupID { get; set; } = string.Empty;
    public string XASPSPCode { get; set; } = string.Empty;
    public string XTPPCode { get; set; } = string.Empty;
    public string PSUInitiated { get; set; } = string.Empty;

    public string? UserReference { get; set; }
    public string? ConsentId { get; set; }
    public string? XJWSSignature { get; set; }
    public string? PSUFraudCheck { get; set; }
}
