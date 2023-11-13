using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class OdemeAyrintilariDto
    {
        public string odmKynk { get; set; }
        public string odmAmc { get; set; }
        public string refBlg { get; set; }
        public string odmAcklm { get; set; }
        public string ohkMsj { get; set; }
        public string odmStm { get; set; }
        public DateTime? bekOdmZmn { get; set; }
    }
}
