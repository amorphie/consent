using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class HesapBilgisiRizaIstegiDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public RizaBilgileriDto rzBlg { get; set; }
        public KimlikDto kmlk { get; set; }
        public KatilimciBilgisiDto katilimciBlg { get; set; }
        public GkdDto gkd { get; set; }
        public HesapBilgisiDto hspBlg { get; set; }
        public string? Description { get; set; }
        public string xGroupId { get; set; }
    }
}
