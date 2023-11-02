using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS
{
    public class HesapBilgisiRizaIstegiHHSDto
    {
        public KatilimciBilgisiDto katilimciBlg { get; set; } = new();
        public GkdRequestDto gkd { get; set; } = new();
        public KimlikDto kmlk { get; set; } = new();
        public HesapBilgisiRequestDto hspBlg { get; set; } = new();
    }
}
