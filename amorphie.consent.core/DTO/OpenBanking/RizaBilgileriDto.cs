using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class RizaBilgileriDto
    {
        public string rizaNo { get; set; }
        public DateTime olusZmn { get; set; }
        public DateTime gnclZmn { get; set; }
        public string rizaDrm { get; set; }
        public string rizaIptDtyKod { get; set; }
    }
}
