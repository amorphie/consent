

using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class KimlikDto
    {
        public string kmlkTur { get; set; } = String.Empty;
        public string kmlkVrs { get; set; } = String.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? krmKmlkTur { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? krmKmlkVrs { get; set; }
        public string ohkTur { get; set; } = String.Empty;
    }
}
