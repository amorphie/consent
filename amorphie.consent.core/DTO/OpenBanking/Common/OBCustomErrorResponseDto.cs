using System.Net;
using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.OpenBanking;

public class OBCustomErrorResponseDto
{
    public OBCustomErrorResponseDto()
    {
        Id = Guid.NewGuid().ToString();
        Timestamp = DateTime.Now;
    }
    public OBCustomErrorResponseDto(HttpStatusCode httpCode, string httpMessage, string path) : this()
    {
        HttpCode = (int)httpCode;
        HttpMessage = httpMessage;
        Path = path;
    }
    public string Path { get; set; } = string.Empty;
    public string Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int HttpCode { get; set; }
    public string HttpMessage { get; set; }= string.Empty;
    public string MoreInformation { get; set; }= string.Empty;
    public string MoreInformationTr { get; set; }= string.Empty;
    [JsonProperty(NullValueHandling = NullValueHandling.Include)]
    public List<FieldError>? FieldErrors { get; set; }
    public string ErrorCode { get; set; }= string.Empty;


}

public class FieldError
{
    public string Field { get; set; }= string.Empty;
    public string MessageTr { get; set; }= string.Empty;
    public string Message { get; set; }= string.Empty;
    public string Code { get; set; }= string.Empty;
}

