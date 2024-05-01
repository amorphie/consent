using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class AccountServiceErrorResponseDto
{
    public string errorCode { get; set; }
    public FieldErrorsAccount fieldErrors { get; set; }
    public int httpCode { get; set; }
    public string httpMessage { get; set; }
    public string moreInformation { get; set; }
    public string moreInformationTr { get; set; }
}

public class FieldErrorsAccount
{
    public string code { get; set; }
    public string field { get; set; }
    public string message { get; set; }
    public string messageTr { get; set; }
    public string objectName { get; set; }
}
