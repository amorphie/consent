using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;
    public class OdemeEmriRizasiHHSDto
    {
        public RizaBilgileriDto rzBlg { get; set; }
        public KatilimciBilgisiDto katilimciBlg { get; set; }
        public GkdDto gkd { get; set; }
        public OdemeBaslatmaDto odmBsltm { get; set; }
        public IsyeriOdemeBilgileriDto isyOdmBlg { get; set; }
        public string xGroupId { get; set; }
    }
