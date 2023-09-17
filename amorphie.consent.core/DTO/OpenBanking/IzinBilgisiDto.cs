using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class IzinBilgisiDto
    {
        public string[] iznTur { get; set; }
        public string[] hspRef { get; set; }
        public DateTime erisimIzniSonTrh { get; set; }
        public DateTime? hesapIslemBslZmn { get; set; }
        public DateTime? hesapIslemBtsZmn { get; set; }
    }
}
