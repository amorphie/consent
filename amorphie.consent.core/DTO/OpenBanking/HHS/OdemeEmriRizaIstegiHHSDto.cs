using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class OdemeEmriRizaIstegiHHSDto
{
    public KatilimciBilgisiDto katilimciBlg { get; set; }
    public GkdRequestDto gkd { get; set; }
    public OdemeBaslatmaRequestDto odmBsltm { get; set; }
    public IsyeriOdemeBilgileriDto? isyOdmBlg { get; set; }
    public string xGroupId { get; set; }
}
