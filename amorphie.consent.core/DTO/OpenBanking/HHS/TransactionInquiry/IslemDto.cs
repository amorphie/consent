using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class IslemDto
{
    public IslemTemelDto islTml { get; set; }
    public IslemDetayDto? islDty { get; set; }
}
