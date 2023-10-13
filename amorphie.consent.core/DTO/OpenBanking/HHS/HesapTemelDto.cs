using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS
{
    public class HesapTemelDto
    {
        public string hspRef { get;  set;  }
        public string? hspNo { get;  set;  }
        public string hspShb { get; set; }
        public string? subeAdi { get;  set;  }
        public string? kisaAd { get;  set;  }
        public string prBrm { get;  set;  }
        public string hspTur { get; set; }
        public string hspTip { get; set; }
        public string? hspUrunAdi { get; set; }
        public string hspDrm { get; set; }
    }
}
