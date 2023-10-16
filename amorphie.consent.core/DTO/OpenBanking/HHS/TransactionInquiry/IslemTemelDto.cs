using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class IslemTemelDto
{
    public string islNo { get; set; }
    public string refNo { get; set; }
    public string islTtr { get; set; }
    public string prBrm { get; set; }
    public DateTime islGrckZaman { get; set; }
    public string kanal { get; set; }
    public string brcAlc { get; set; }
    public string islTur { get; set; }
    public string islAmc { get; set; }
    public string? odmStmNo { get; set; }
}
