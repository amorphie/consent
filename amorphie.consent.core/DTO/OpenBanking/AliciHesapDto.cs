﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking
{
    public class AliciHesapDto
    {
        public string unv { get; set; }
        public string hspNo { get; set; }
        public KolasDto? kolas { get; set; }
    }
}
