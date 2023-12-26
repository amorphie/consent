using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.Contract.ContractInstance;

public class DocumentModelTemplateDto
{
    public class DocumentModelTemplate
    {
        public string name { get; set; }
        [JsonProperty("min-version")]
        [JsonPropertyName("min-version")]
        public string minVersion { get; set; }

    }
}