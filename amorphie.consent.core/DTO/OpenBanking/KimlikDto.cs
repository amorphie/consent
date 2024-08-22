

using System.ComponentModel;
using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class KimlikDto
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string kmlkTur { get; set; } = string.Empty;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue("")]
        public string kmlkVrs { get; set; } = string.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? krmKmlkTur { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? krmKmlkVrs { get; set; }
        public string ohkTur { get; set; } = String.Empty;
    }
}
