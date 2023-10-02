using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS
{
    public class HesapBilgisiRizaIstegiHHSDto
    {
        public KatilimciBilgisiDto katilimciBlg { get; set; }
        public GkdRequestDto gkd { get; set; }
        public KimlikDto kmlk { get; set; }
        public HesapBilgisiRequestDto hspBlg { get; set; }
        public string xGroupId { get; set; }
    }
}
