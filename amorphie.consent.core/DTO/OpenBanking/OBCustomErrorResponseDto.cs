using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.OpenBanking;

public class OBCustomErrorResponseDto
{
    public string Path { get; set; }
    public string Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int HttpCode { get; set; }
    public string HttpMessage { get; set; }
    public string MoreInformation { get; set; }
    public string MoreInformationTr { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Include)]
    public List<FieldError> FieldErrors { get; set; }
    public string ErrorCode { get; set; }


}

public class FieldError
{
    public string ObjectName { get; set; }
    public string Field { get; set; }
    public string MessageTr { get; set; }
    public string Message { get; set; }
    public string Code { get; set; }
}

