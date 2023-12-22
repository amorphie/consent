using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.Contract;

public class DocumentModelTemplateDto
{
    public class DocumentModelTemplate {
        public string name { get; set; }
        [JsonProperty("min-version")]
        public string minVersion { get; set; }

    }
}