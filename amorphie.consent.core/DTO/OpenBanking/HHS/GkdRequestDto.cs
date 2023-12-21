using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS
{
    public class GkdRequestDto
    {
        public string? yetYntm { get; set; }
        public string yonAdr { get; set; }
        public AyrikGkdDto? ayrikGkd { get; set; }
    }
}
