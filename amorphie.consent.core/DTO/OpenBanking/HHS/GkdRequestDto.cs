

using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class GkdRequestDto
{
    public string? yetYntm { get; set; }
    public string yonAdr { get; set; } = String.Empty;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public AyrikGkdDto? ayrikGkd { get; set; }
}