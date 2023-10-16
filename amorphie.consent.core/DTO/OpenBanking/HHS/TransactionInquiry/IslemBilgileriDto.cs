using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class IslemBilgileriDto
{
    public string hspRef { get; set; }
    public List<IslemDto>? isller { get; set; }
}
