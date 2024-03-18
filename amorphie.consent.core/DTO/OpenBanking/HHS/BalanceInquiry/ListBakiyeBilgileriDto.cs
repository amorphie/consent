using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class ListBakiyeBilgileriDto
{
    public List<BakiyeBilgileriDto>? bakiyeBilgileri { get; set; }
    public int toplamBakiyeSayisi { get; set; }
}
