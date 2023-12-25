using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.Contract.DocumentInstance;

public class DocumentInstanceRequestDto
{
    public Guid id { get; set; } = Guid.NewGuid();
    [JsonProperty("file-type")]
    [JsonPropertyName("file-type")]
    public string fileType { get; set; }
    public string fileContextType { get; set; }
    [JsonProperty("file-name")]
    [JsonPropertyName("file-name")]
    public string fileName { get; set; }
    public string documentCode { get; set; }
    public string documentVersion { get; set; }
    public string reference { get; set; }
    public string owner { get; set; }
    public string fileContext { get; set; }
}