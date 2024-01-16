using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.Contract.TemplateRender;

public class TemplateRenderRequestDto
{
    public TemplateRenderRequestDto(Guid renderId,
        string templateName,
        string minVersion)
    {
        name = templateName;
        semanticVersion = minVersion;
        this.renderId = renderId;
        renderData = string.Empty;
        renderDataForLog = string.Empty;
    }
    public string name { get; set; }
    [JsonProperty("render-id")]
    [JsonPropertyName("render-id")]
    public Guid renderId { get; set; }
    [JsonProperty("render-data")]
    [JsonPropertyName("render-data")]
    public string renderData { get; set; }
    [JsonProperty("render-data-for-log")]
    [JsonPropertyName("render-data-for-log")]
    public string renderDataForLog { get; set; }
    [JsonProperty("semantic-version")]
    [JsonPropertyName("semantic-version")]
    public string semanticVersion { get; set; }
}
