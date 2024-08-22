using amorphie.consent.core.Helper;
using Newtonsoft.Json;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class EmirBilgileriDto
{
    public string odmEmriNo { get; set; }
     [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime odmEmriZmn { get; set; }

}