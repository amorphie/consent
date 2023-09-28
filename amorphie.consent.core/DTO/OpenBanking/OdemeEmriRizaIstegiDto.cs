using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class OdemeEmriRizaIstegiDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public RizaBilgileriDto rzBlg { get; set; }
        public KatilimciBilgisiDto katilimciBlg { get; set; }
        public GkdDto gkd { get; set; }
        public OdemeBaslatmaDto odmBsltm { get; set; }
        public IsyeriOdemeBilgileriDto isyOdmBlg { get; set; }
        public string? Description { get; set; }
        public string xGroupId { get; set; }
    }
}
