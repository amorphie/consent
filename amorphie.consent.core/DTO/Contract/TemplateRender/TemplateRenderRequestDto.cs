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
        renderData = "{}";
        renderDataForLog = string.Empty;
        processName = string.Empty;
        itemId = action = identity = customer = childrenName = string.Empty;
        children = new Collection<string>()
                {
                        "string"
                };
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
    [JsonProperty("process-name")]
    [JsonPropertyName("process-name")]
    public string processName { get; set; }
    [JsonProperty("item-id")]
    [JsonPropertyName("item-id")]
    public string itemId { get; set; }
    public string action { get; set; }
    public string identity { get; set; }
    public string customer { get; set; }
    [JsonProperty("children-name")]
    public string childrenName { get; set; }
    public IList<string> children { get; set; }
}
