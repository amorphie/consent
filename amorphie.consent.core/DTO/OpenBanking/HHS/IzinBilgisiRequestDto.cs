using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS
{
    public class IzinBilgisiRequestDto
    {
        public string[] iznTur { get; set; }
        public DateTime erisimIzniSonTrh { get; set; }
        public DateTime? hesapIslemBslZmn { get; set; }
        public DateTime? hesapIslemBtsZmn { get; set; }
    }
}
