using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace amorphie.consent.core.DTO.OpenBanking.HHS;

/// <summary>
/// OdemeBaslatma response object for Odeme Emri
/// </summary>
public class OdemeBaslatmaResponseOEDto
{
    public KimlikDto kmlk { get; set; }
    public TutarDto islTtr { get; set; }
    public GonderenHesapDto gon { get; set; }
    public AliciHesapDto alc { get; set; }
    public KarekodDto? kkod { get; set; }
    public OdemeAyrintilariDetailedDto odmAyr { get; set; }
    public TutarDto? hhsMsrfTtr { get; set; }
}
