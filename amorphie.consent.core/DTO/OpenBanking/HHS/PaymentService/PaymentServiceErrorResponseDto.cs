using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.OpenBanking.PaymentService;

public class PaymentServiceErrorResponseDto
{
    public string errorCode { get; set; }
    public FieldErrors fieldErrors { get; set; }
    public int httpCode { get; set; }
    public string httpMessage { get; set; }
    public string moreInformation { get; set; }
    public string moreInformationTr { get; set; }
}

public class FieldErrors
{
    public string code { get; set; }
    public string field { get; set; }
    public string message { get; set; }
    public string messageTr { get; set; }
    public string objectName { get; set; }
}
