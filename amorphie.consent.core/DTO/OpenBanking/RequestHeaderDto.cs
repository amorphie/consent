using Microsoft.AspNetCore.Mvc;

namespace amorphie.consent.core.DTO.OpenBanking;
public class RequestHeaderDto
{
    [FromHeader]
    public string ContentType { get; set; }
    [FromHeader] public string XRequestID { get; set; }
    [FromHeader] public string XGroupID { get; set; }
    [FromHeader] public string XASPSPCode { get; set; }
    [FromHeader] public string XTPPCode { get; set; }
    [FromHeader] public string PSUInitiated { get; set; }
}
