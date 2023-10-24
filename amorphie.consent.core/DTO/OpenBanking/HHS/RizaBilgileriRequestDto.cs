using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking;
    public class RizaBilgileriRequestDto
{
    public string rizaNo { get; set; }
    public DateTime olusZmn { get; set; }
    public string rizaDrm { get; set; }
}
