using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.Contract.ContractInstance;

public class DocumentInfoDto
{
    public string code { get; set; }
    public string title { get; set; }
    public string status { get; set; }
    public bool required { get; set; }
    public bool render { get; set; }
    [JsonProperty("online-sign")]
    [JsonPropertyName("online-sign")]
    public OnlineSignDto.OnlineSign onlineSign { get; set; } 
    public string version { get; set; }
}