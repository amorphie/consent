using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class GkdDto
    {
        public string yetYntm { get; set; }
        public string yonAdr { get; set; }
        public AyrikGkdDto ayrikGkd { get; set; }
        public string hhsYonAdr { get; set; }
        public DateTime yetTmmZmn { get; set; }
    }
}
