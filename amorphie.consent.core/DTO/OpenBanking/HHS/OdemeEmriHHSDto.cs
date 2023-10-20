using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class OdemeEmriHHSDto
{
    public RizaBilgileriRequestDto rzBlg { get; set; }
    public KatilimciBilgisiDto katilimciBlg { get; set; }
    public GkdDto gkd { get; set; }
    public EmirBilgileriDto emrBlg { get; set; }
    public OdemeBaslatmaResponseOEDto odmBsltm { get; set; }
    public IsyeriOdemeBilgileriDto? isyOdmBlg { get; set; }
}
