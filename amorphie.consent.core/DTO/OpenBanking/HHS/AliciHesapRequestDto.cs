using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;
    public class AliciHesapRequestDto
    {
        public string unv { get; set; }
        public string hspNo { get; set; }
        public KolasRequestDto? kolas { get; set; }
    }
