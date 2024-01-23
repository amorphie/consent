using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class OdemeBaslatmaWithMsrfTtrDto
    {
        public KimlikDto kmlk { get; set; }
        public TutarDto islTtr { get; set; }
        public GonderenHesapDto gon { get; set; }
        public AliciHesapDto alc { get; set; }
        public KarekodDto? kkod { get; set; }
        public OdemeAyrintilariDto odmAyr { get; set; }
        public TutarDto? hhsMsrfTtr { get; set; }

    }
}
