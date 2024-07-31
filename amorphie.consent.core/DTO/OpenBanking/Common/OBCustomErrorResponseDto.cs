using System.Globalization;
using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.OpenBanking;

public class OBCustomErrorResponseDto
{
    public OBCustomErrorResponseDto()
    {
        Id = Guid.NewGuid().ToString();
        Timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture);

    }
    public OBCustomErrorResponseDto(HttpStatusCode httpCode, string httpMessage, string path) : this()
    {
        HttpCode = (int)httpCode;
        HttpMessage = httpMessage;
        Path = path;
    }
    [JsonProperty("path")]
    public string Path { get; set; } = string.Empty;

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("timestamp")]
    public string Timestamp { get; set; }

    [JsonProperty("httpCode")]
    public int HttpCode { get; set; }

    [JsonProperty("httpMessage")]
    public string HttpMessage { get; set; } = string.Empty;

    [JsonProperty("moreInformation")]
    public string MoreInformation { get; set; } = string.Empty;

    [JsonProperty("moreInformationTr")]
    public string MoreInformationTr { get; set; } = string.Empty;

    [JsonProperty("fieldErrors", NullValueHandling = NullValueHandling.Ignore)]
    public List<FieldError>? FieldErrors { get; set; }

    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; } = string.Empty;

}

public class FieldError
{
    [JsonProperty("objectName", NullValueHandling = NullValueHandling.Ignore)]
    public string? ObjectName { get; set; }

    [JsonProperty("field")]
    public string Field { get; set; } = string.Empty;

    [JsonProperty("messageTr")]
    public string MessageTr { get; set; } = string.Empty;

    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;
}

