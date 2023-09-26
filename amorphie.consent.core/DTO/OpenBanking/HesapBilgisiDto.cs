using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class HesapBilgisiDto
    {
        public IzinBilgisiDto iznBlg { get; set; }
        public AyrintiBilgiDto ayrBlg { get; set; }
        public string ohkMsj { get; set; }
    }
}
