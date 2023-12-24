using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.Contract.ContractInstance;

public class OnlineSignDto
{
    public class OnlineSign {
        [JsonProperty("sca-required")]
        public bool scaRequired { get; set; }
        [JsonProperty("allowed-clients")]
        public IList<string> allowedClients { get; set; }
        [JsonProperty("document-model-template")]
        public IList<DocumentModelTemplateDto.DocumentModelTemplate> documentModelTemplate { get; set; }
    }
}