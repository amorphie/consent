using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class HesapBilgileriDto
{
    public string rizaNo { get; set; }
    public HesapTemelDto hspTml { get; set; }
    public HesapDetayDto? hspDty { get; set; }
}
